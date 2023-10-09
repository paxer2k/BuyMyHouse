using AutoMapper;
using DAL;
using DAL.Repository.Interfaces;
using Domain;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IMortgageService _mortgageService;
        private readonly ICustomerService _customerService;
        private readonly AppConfiguration _appConfiguration;

        public EmailService(IMortgageService mortgageService, ICustomerService customerService, AppConfiguration appConfiguration)
        {
            _mortgageService = mortgageService;
            _customerService = customerService;
            _appConfiguration = appConfiguration;
        }

        public async Task SendEmail()
        {
            var customers = await _customerService.GetAllCustomersAsync();

            foreach(var customer in customers)
            {
                if (!await _mortgageService.IsMortgageSent(customer.Id))
                    continue;

                await GenerateEmail(customer); // only generate an email for those who have sent an application
            }
        }

        private async Task GenerateEmail(Customer customer)
        {
            string gridApiKey = _appConfiguration.GridApiKey;
            SendGridClient gridClient = new SendGridClient(gridApiKey);

            EmailAddress sender = new EmailAddress(_appConfiguration.MailSender, "BuyMyHouse.co");
            string subject = "Your mortgage application review";

            EmailAddress receiver = new EmailAddress(customer.Email, $"{customer.FirstName} {customer.LastName}");
            string plainTextContent = $"Dear Mr/Ms {customer.FirstName}, hereby you can view your mortgage. The following link will be available for the next 24 hours.";
            string htmlContent = $"Dear {customer.FirstName}, <br><br> The maximum worth of your mortgage can be seen by following this link. This link is available for 24 hours. </n> <a href='http://localhost:7132/api/mortgages/{customer.Id}'>Click Here</a>";

            SendGridMessage message = MailHelper.CreateSingleEmail(sender, receiver, subject, plainTextContent, htmlContent);

            // set expiry time for the mortgage
            var mortgage = await _mortgageService.GetMortgageByCustomerIdAsync(customer.Id);
            mortgage.ExpiresAt = DateTime.Now.AddHours(24);
            await _mortgageService.UpdateMortgageAsync(mortgage); // update mortgage

            await gridClient.SendEmailAsync(message);
        }
    }
}
