using QRCoder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;

public class QRService
{
    private readonly string _sendGridApiKey;
    private readonly ILogger<QRService> _logger;

    public QRService(IConfiguration configuration, ILogger<QRService> logger)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
        _logger = logger;
    }

    public byte[] GenerateQrCode(string data)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            using (var qrCodeImage = qrCode.GetGraphic(20))
            {
                using (var stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }

    public async Task SendEmailWithQrCodeAsync(string email, string subject, string message, byte[] qrCodeImage)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("your-email@example.com", "Your Name");
            var to = new EmailAddress(email);
            var plainTextContent = message;
            var htmlContent = $"<p>{message}</p><img src='cid:qrCodeImage'/>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var qrCodeAttachment = new Attachment
            {
                Content = Convert.ToBase64String(qrCodeImage),
                Type = "image/png",
                Filename = "qrcode.png",
                Disposition = "inline",
                ContentId = "qrCodeImage"
            };
            msg.AddAttachment(qrCodeAttachment);

            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation($"Email sent to {email} with status code {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send email to {email}: {ex.Message}");
        }
    }
}
