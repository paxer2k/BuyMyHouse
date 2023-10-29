using Domain;
using Domain.Configuration.Interfaces;
using SendGrid.Helpers.Mail;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;
using Domain.Enums;

namespace Service.Commands
{
    public class EmailCommandService : IEmailCommandService
    {
        private readonly IMortgageQueryService _mortgageQueryService;
        private readonly IMortgageCommandService _mortgageCommandService;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ISendGridMailerCommandService _sendGridMailerCommandService;

        public EmailCommandService(IMortgageQueryService mortgageQueryService, IMortgageCommandService mortgageCommandService, IAppConfiguration appConfiguration, ISendGridMailerCommandService sendGridMailerCommandService)
        {
            _mortgageQueryService = mortgageQueryService;
            _mortgageCommandService = mortgageCommandService;
            _appConfiguration = appConfiguration;
            _sendGridMailerCommandService = sendGridMailerCommandService;
        }

        public async Task SendEmailsAsync()
        {
            var processedMortgages = await _mortgageQueryService.GetProcessedMortgages();

            foreach (var mortgage in processedMortgages)
            {
                foreach (var customer in mortgage.Customers)
                {
                    await SendEmailAsync(customer, mortgage);
                }

                // update mortgage expiry date after sending mail because otherwise this will be done twice (if there are two customers)
                await UpdateMortgageInformationAsync(mortgage);
            }
        }

        private async Task SendEmailAsync(Customer customer, Mortgage mortgage)
        {
            var from = new EmailAddress(_appConfiguration.SendGridConfig.Sender, "BuyMyHouse.co");
            var subject = $"Your mortgage application has been {mortgage.MortgageStatus.ToString().ToUpper()}";
            var to = new EmailAddress(customer.Email, $"{customer.FirstName} {customer.LastName}");

            string plainTextContent = "";
            string htmlContent = "";

            if (mortgage.MortgageStatus == MortgageStatus.Declined)
            {
                htmlContent = $"<div>Dear {customer.FirstName} {customer.LastName},<br>" +
                    $"<p>After reviewing your mortgage application, we regret to inform you that it has been <strong>declined</strong>.</p>" +
                    $"<p>If you would like to know the reason behind the decline, please use the contact information below:</p>" +
                    $"<p>Contact email: alex.arkhipov.testmail@gmail.com</p>" +
                    $"<p>Contact phone: 068432842</p>" +
                    $"<p>Kind regards,</p>" +
                    $"<p>BuyMyHouse.co</p></div>";
            }

            if (mortgage.MortgageStatus == MortgageStatus.Approved)
            {
                htmlContent = $"<div>Dear {customer.FirstName} {customer.LastName},<br>" +
                    $"<p>After reviewing your application, we are pleased to inform you that it has been <strong>approved</strong>.</p>" +
                    $"<p>You can view your mortgage application through this <a href=\"{_appConfiguration.SendGridConfig.MortgageApplicationEndpoint}{mortgage.Id}\">link</a>.</p>" +
                    $"<p><strong>NOTE:</strong> The link to your mortgage application will be valid for the next 24 hours.</p>" +
                    $"<p>Kind regards,</p>" +
                    $"<p>BuyMyHouse.co</p></div>";
            }

            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await _sendGridMailerCommandService.SendEmailAsync(message);
        }

        private async Task UpdateMortgageInformationAsync(Mortgage mortgage)
        {
            mortgage.ExpiresAt = DateTime.Now.AddDays(1);
            mortgage.IsEmailSent = true;
            await _mortgageCommandService.UpdateMortgageAsync(mortgage);
        }
    }
}
