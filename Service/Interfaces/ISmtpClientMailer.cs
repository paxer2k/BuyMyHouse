using System.Net.Mail;

namespace Service.Interfaces
{
    public interface ISmtpClientMailer
    {
        Task SendEmailAsync(MailMessage message);
    }
}
