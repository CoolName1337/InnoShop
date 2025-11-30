using Infrastructure;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;

namespace Infrastructure.Tests
{
    [TestClass()]
    public class EmailServiceTests
    {
        EmailService emailService;

        [TestInitialize]
        public void Initialize()
        {
            var mockLogger = new Mock<ILogger<EmailService>>();
            var smtpOptions = new SmtpOptions();
            
            emailService = new EmailService(new OptionsWrapper<SmtpOptions>(smtpOptions), mockLogger.Object);
        }
        [TestMethod()]
        public async Task SendEmailAsyncTest_InvalideArguments_ThrowParseException()
        {
            Dictionary<string, string> sendData = new()
            {
                {"toEmail", "incorrect2email..com" },
                {"subject", "Norm" },
                {"html", "<a href='https://www.youtube.com/watch?v=dQw4w9WgXcQ'>жми сюда</a>" }
            };
            await Assert.ThrowsExceptionAsync<ParseException>(
                async () => await emailService.SendEmailAsync(
                    sendData["toEmail"], 
                    sendData["subject"], 
                    sendData["html"])
                );
        }
    }
}