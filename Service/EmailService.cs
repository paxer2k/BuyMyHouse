﻿using DAL.Interfaces;
using Domain;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.Interfaces;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IMortgageService _mortgageService;
        private readonly ICustomerService _customerService;
        private readonly IAppConfiguration _appConfiguration;

        public EmailService(IMortgageService mortgageService, ICustomerService customerService, IAppConfiguration appConfiguration)
        {
            _mortgageService = mortgageService;
            _customerService = customerService;
            _appConfiguration = appConfiguration;
        }

        public async Task SendEmail()
        {
            var mortgages = await _mortgageService.GetAllActiveMortgages();

            foreach(var mortgage in mortgages)
            {
                foreach(var customer in mortgage.Customers)
                {
                    await GenerateEmail(customer, mortgage); // only generate an email for those who have sent an application
                }
            }
        }

        private async Task GenerateEmail(Customer customer, Mortgage activeMortgage)
        {
            string gridApiKey = _appConfiguration.MailerConfig.GridApiKey;
            SendGridClient gridClient = new SendGridClient(gridApiKey);

            EmailAddress sender = new EmailAddress(_appConfiguration.MailerConfig.MailSender, "BuyMyHouse.co");
            string subject = "BuyMyHouse.co | Your mortgage application";

            EmailAddress receiver = new EmailAddress(customer.Email, $"{customer.FirstName} {customer.LastName}");
            string plainTextContent = $"Dear Mr/Ms {customer.FirstName}, hereby you can view your mortgage. The following link will be available for the next 24 hours.";
            string htmlContent = $"Dear {customer.FirstName}, <br><br> The mortgage application can be viewed under the following link. This link is available for the next 24 hours. </n> <a href='http://localhost:7132/api/mortgages/{activeMortgage.Id}'>Click Here</a>";

            SendGridMessage message = MailHelper.CreateSingleEmail(sender, receiver, subject, plainTextContent, htmlContent);

            activeMortgage.ExpiresAt = DateTime.Now.AddHours(24);
            await _mortgageService.UpdateMortgageAsync(activeMortgage); // update mortgage

            await gridClient.SendEmailAsync(message);
        }
    }
}
