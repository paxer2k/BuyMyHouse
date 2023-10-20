using SendGrid.Helpers.Mail;

namespace Service.Command.Interfaces
{
    public interface ISendGridMailerCommandService
    {
        Task SendEmailAsync(SendGridMessage message);
    }
}
