using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using Medicina.MailUtil;

namespace Medicina.Service
{
    public class EmailService
    {

        private readonly MailSettings _settings;
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _settings = mailSettingsOptions.Value;

            _smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_settings.From, _settings.Password),
                EnableSsl = true
            };

            _senderEmail = _settings.From;
            _senderPassword = _settings.Password;
        }

     
        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var message = new MailMessage(_senderEmail, recipient, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                await _smtpClient.SendMailAsync(message);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                message.Dispose();
            }
        }
    }
}
