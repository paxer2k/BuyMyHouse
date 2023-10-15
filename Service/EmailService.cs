using DAL.Configuration.Interfaces;
using Domain;
using Service.Interfaces;
using System.Net;
using System.Net.Mail;

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

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_appConfiguration.MailerConfig.MailSender);
            mailMessage.To.Add(customer.Email);
            mailMessage.Subject = "BuyMyHouse.co | Your mortgage application";
            mailMessage.Body = $"<div><strong>Thank you for using BuyMyHouse!</strong><br>" +
                                                $"<p>Through <a href=https://localhost:7217/mortgages/{activeMortgage.Id}>this link</a> you can view your personal mortgage offer.<br>The link will be available for 24 hours</p></div>";
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            NetworkCredential netCre = new NetworkCredential(_appConfiguration.MailerConfig.MailSender, _appConfiguration.MailerConfig.MailPassword);
            smtpClient.Credentials = netCre;
            smtpClient.UseDefaultCredentials = false;

            await smtpClient.SendMailAsync(mailMessage);      
        }
    }
}
