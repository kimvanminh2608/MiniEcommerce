using Contracts.Configurations;
using Contracts.Services;
using Infrastructure.Configuation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MimeKit;
using Serilog;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    // Google mail
    public class SMTPEmailService : ISMTPEmailService
    {
        private readonly ILogger _logger;
        private readonly SMTPEmailSetting _settings;
        private readonly SmtpClient _smtpClient;
        public SMTPEmailService(ILogger logger, SMTPEmailSetting settings)
        {
            _logger = logger;
            _settings = settings;
            _smtpClient = new SmtpClient();
        }
        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.DisplayName, request.FromAddress ?? _settings.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body,
                }.ToMessageBody()
            };

            if (request.ToAddresses.Any())
            {
                foreach (var address in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(address));
                }
            }
            else
            {
                var toAddress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }

            try
            {
                await _smtpClient.ConnectAsync(_settings.SmtpServer, _settings.port, _settings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                
                await _smtpClient.SendAsync(emailMessage);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }
        }
    }
}
