using QRCoder;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class MailgunService
{
    private readonly string _apiKey;
    private readonly string _domain;
    private readonly ILogger<MailgunService> _logger;
    private readonly HttpClient _httpClient;

    public MailgunService(IConfiguration configuration, ILogger<MailgunService> logger)
    {
        _apiKey = configuration["Mailgun:ApiKey"];
        _domain = configuration["Mailgun:Domain"];
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task SendEmailWithQrCodeAsync(string toEmail, string subject, string message, byte[] qrCodeImage)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(toEmail), "to" },
                { new StringContent($"Mailgun Sandbox <postmaster@{_domain}>"), "from" },
                { new StringContent(subject), "subject" },
                { new StringContent(message), "text" },
                { new StringContent($"<html><p>{message}</p><img src='cid:qrcode.png'></html>"), "html" },
                { new ByteArrayContent(qrCodeImage) { Headers = { ContentType = new MediaTypeHeaderValue("image/png") } }, "inline", "qrcode.png" }
            };

            var byteArray = System.Text.Encoding.ASCII.GetBytes($"api:{_apiKey}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await _httpClient.PostAsync($"https://api.mailgun.net/v3/{_domain}/messages", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Email sent to {toEmail} with status code {response.StatusCode}");
            }
            else
            {
                _logger.LogError($"Failed to send email to {toEmail}. Status Code: {response.StatusCode}");
                _logger.LogError($"Response: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send email to {toEmail}: {ex.Message}");
        }
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
                    qrCodeImage.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }
}
