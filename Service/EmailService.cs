using DAL.Configuration.Interfaces;
using Domain;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.Interfaces;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IMortgageService _mortgageService;
        private readonly IAppConfiguration _appConfiguration;

        public EmailService(IMortgageService mortgageService, IAppConfiguration appConfiguration)
        {
            _mortgageService = mortgageService;
            _appConfiguration = appConfiguration;
        }

        public async Task SendEmails()
        {
            var mortgages = await _mortgageService.GetMortgagesOfToday(); // CHANGE THIS BACK LATER GetActiveMortgagesOfYesterday();

            foreach (var mortgage in mortgages)
            {
                foreach(var customer in mortgage.Customers)
                {
                    await GenerateEmail(customer, mortgage); // only generate an email for those who have sent an application
                }
            }
        }

        private async Task GenerateEmail(Customer customer, Mortgage activeMortgage)
        {
            string gridApiKey = _appConfiguration.MailerConfig.GridApiKey!;
            SendGridClient gridClient = new SendGridClient(gridApiKey);

            EmailAddress sender = new EmailAddress(_appConfiguration.MailerConfig.MailSender, "BuyMyHouse.co");
            string subject = "BuyMyHouse.co | Your mortgage application";

            EmailAddress receiver = new EmailAddress(customer.Email, $"{customer.FirstName} {customer.LastName}");
            string plainTextContent = $"Dear {customer.FirstName} {customer.LastName}, thank you for your interest in BuyMyHouse. Through this link you can view your personal mortgage offer";

            string htmlContent = $"<div><strong>Thank you for using BuyMyHouse!</strong><br>" +
                                    $"<p>Through <a href=http://localhost:7132/api/mortgages/{activeMortgage.Id}>this link</a> you can view your personal mortgage offer.<br>The link will be available for 24 hours</p></div>";

            SendGridMessage message = MailHelper.CreateSingleEmail(sender, receiver, subject, plainTextContent, htmlContent);

            activeMortgage.ExpiresAt = DateTime.Now.AddDays(1);
            await _mortgageService.UpdateMortgageAsync(activeMortgage); // update mortgage by setting the expiry time

            await gridClient.SendEmailAsync(message);
        }
    }
}
