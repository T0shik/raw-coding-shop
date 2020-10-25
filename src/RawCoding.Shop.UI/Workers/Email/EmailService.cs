using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RawCoding.Shop.Application.Emails;

namespace RawCoding.Shop.UI.Workers.Email
{
    public class EmailService : BackgroundService
    {
        private readonly IOptionsMonitor<EmailSettings> _optionsMonitor;
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailQueue _emailQueue;

        public EmailService(
            IOptionsMonitor<EmailSettings> optionsMonitor,
            ILogger<EmailService> logger,
            IEmailQueue emailQueue)
        {
            _optionsMonitor = optionsMonitor;
            _logger = logger;
            _emailQueue = emailQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var request = await _emailQueue.ReadAsync();

                    _logger.LogInformation("Sending Email to {0} , with subject {1}", request.To, request.Subject);

                    var settings = _optionsMonitor.CurrentValue;
                    using var smtp = CreateClient(settings);
                    using var mailMessage = CreateMessage(settings, request);
                    await smtp.SendMailAsync(mailMessage);
                }
                catch (SmtpException e)
                {
                    _logger.LogError(e, "Failed to send email");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to send email");
                }
            }
        }

        private static SmtpClient CreateClient(EmailSettings settings)
        {
            return new SmtpClient
            {
                Host = settings.Host,
                Port = int.Parse(settings.Port),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(settings.Account, settings.Password),
            };
        }

        private static MailMessage CreateMessage(EmailSettings settings, SendEmailRequest request)
        {
            return new MailMessage(settings.SenderEmail, request.To)
            {
                Subject = request.Subject,
                Body = request.Message,
                IsBodyHtml = request.Html,
            };
        }
    }
}