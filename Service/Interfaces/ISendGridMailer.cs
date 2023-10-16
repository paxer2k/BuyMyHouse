using SendGrid.Helpers.Mail;

namespace Service.Interfaces
{
    public interface ISendGridMailer
    {
        Task SendEmailAsync(SendGridMessage message);
    }
}
