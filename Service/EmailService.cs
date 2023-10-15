using Domain;
using Domain.Configuration.Interfaces;
using Service.Interfaces;
using System.Net.Mail;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IMortgageService _mortgageService;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ISmtpClientMailer _smtpClientMailer;

        public EmailService(IMortgageService mortgageService, IAppConfiguration appConfiguration, ISmtpClientMailer smtpClientMailer)
        {
            _mortgageService = mortgageService;
            _appConfiguration = appConfiguration;
            _smtpClientMailer = smtpClientMailer;
        }

        public async Task SendEmailsAsync()
        {
            var mortgages = await _mortgageService.GetMortgagesOfToday(); // CHANGE THIS BACK LATER GetActiveMortgagesOfYesterday();

            foreach (var mortgage in mortgages)
            {
                foreach(var customer in mortgage.Customers)
                {
                    await SendMortgageOfferEmailAsync(customer, mortgage);
                }
            }
        }

        private async Task SendMortgageOfferEmailAsync(Customer customer, Mortgage activeMortgage)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_appConfiguration.SmtpConfig.Sender);
            mailMessage.To.Add(customer.Email);
            mailMessage.Subject = "BuyMyHouse.co | Your mortgage application";
            mailMessage.Body = $"<div><strong>Thank you for using BuyMyHouse!</strong><br>" +
                                                $"<p>Through <a href=https://localhost:7217/mortgages/{activeMortgage.Id}>this link</a> you can view your personal mortgage offer.<br>The link will be available for 24 hours</p></div>";
            mailMessage.IsBodyHtml = true;

            await _smtpClientMailer.SendEmailAsync(mailMessage);
        }
    }
}
