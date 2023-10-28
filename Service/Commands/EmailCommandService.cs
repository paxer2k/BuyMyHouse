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
        private readonly IMortgageQueryService _mortgageQuery;
        private readonly IMortgageCommandService _mortgageCommand;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ISendGridMailerCommandService _sendGridMailerCommandService;

        public EmailCommandService(IMortgageQueryService mortgageQuery, IMortgageCommandService mortgageCommand, IAppConfiguration appConfiguration, ISendGridMailerCommandService sendGridMailerCommandService)
        {
            _mortgageQuery = mortgageQuery;
            _mortgageCommand = mortgageCommand;
            _appConfiguration = appConfiguration;
            _sendGridMailerCommandService = sendGridMailerCommandService;
        }

        public async Task SendEmailsAsync()
        {
            var finishedMortgages = await _mortgageQuery.GetFinishedMortgages();

            foreach (var mortgage in finishedMortgages)
            {
                foreach (var customer in mortgage.Customers)
                {
                    await SendEmailAsync(customer, mortgage);
                }

                // update mortgage expiry date after sending mail because otherwise this will be done twice (if there are two customers)
                await UpdateMortgageInformationAsync(mortgage);
            }
        }

        private async Task SendEmailAsync(Customer customer, Mortgage activeMortgage)
        {
            var from = new EmailAddress(_appConfiguration.SendGridConfig.Sender, "BuyMyHouse");
            var subject = "BuyMyHouse.co | Your mortgage application";
            var to = new EmailAddress(customer.Email, $"{customer.FirstName} {customer.LastName}");
            var plainTextContent = "Hereby your mortgage offer:";
            var htmlContent = $"<div><strong>Thank you for using BuyMyHouse!</strong><br>" +
                $"<p>Through <a href=https://localhost:7217/mortgages/{activeMortgage.Id}>this link</a> you can view your personal mortgage offer.<br>The link will be available for 24 hours</p></div>";

            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await _sendGridMailerCommandService.SendEmailAsync(message);
        }

        private async Task UpdateMortgageInformationAsync(Mortgage mortgage)
        {
            mortgage.ExpiresAt = DateTime.Now.AddDays(1);
            mortgage.IsEmailSent = true;
            await _mortgageCommand.UpdateMortgageAsync(mortgage);
        }
    }
}
