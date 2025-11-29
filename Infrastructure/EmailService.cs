using Infrastructure.Interfaces;
using Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure
{
    public class EmailService(
        IOptions<SmtpOptions> options, 
        ILogger<EmailService> logger) : IEmailService
    {
        private readonly SmtpOptions _smtpOptions = options.Value;
        private readonly string _fromEmail = "InnoShop@no-reply.com";
        public async Task SendConfirmationLinkAsync(string toEmail, string link)
        {
            await SendEmailAsync(
                toEmail,
                "Верификация почты в InnoShop",
                $"Подтвердите свою почту нажав <a href='{link}'>сюда</a>");
        }

        public async Task SendRecoveryLinkAsync(string toEmail, string link)
        {
            await SendEmailAsync(
                toEmail,
                "Восстановление аккаунта в InnoShop",
                $"Для создания нового пароля жмите <a href='{link}'>сюды</a>");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string html)
        {
            logger.LogInformation("[EmailService]SendEmailAsync TRY TO {toEmail}", toEmail);

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("InnoShop", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = html };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpOptions.Email, _smtpOptions.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            logger.LogInformation("[EmailService]SendEmailAsync CORRECT TO {toEmail}", toEmail);
        }
    }
}
