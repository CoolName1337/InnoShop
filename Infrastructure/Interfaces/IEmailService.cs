namespace Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationLinkAsync(string toEmail, string link);
        Task SendEmailAsync(string toEmail, string subject, string html);
        Task SendRecoveryLinkAsync(string toEmail, string link);
    }
}