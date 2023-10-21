using SendGrid.Helpers.Mail;

namespace Service.Commands.Interfaces
{
    public interface ISendGridMailerCommandService
    {
        Task SendEmailAsync(SendGridMessage message);
    }
}
