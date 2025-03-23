using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Projeto4pServer.Services
{
    public class EmailService
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");
            _server = smtpSettings["Server"]!;
            _port = int.Parse(smtpSettings["Port"]!);
            _username = smtpSettings["Username"]!;
            _password = smtpSettings["Password"]!;
            _fromEmail = smtpSettings["FromEmail"]!;
            _fromName = smtpSettings["FromName"]!;
            _logger = logger;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_server, _port))
                {
                    client.EnableSsl = true; // Habilitar SSL/TLS
                    client.Credentials = new NetworkCredential(_username, _password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_fromEmail, _fromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    client.Send(mailMessage);
                    _logger.LogInformation("E-mail enviado com sucesso para {toEmail}.", toEmail);
                }
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail para {toEmail}.", toEmail);
                throw; // Re-lança a exceção para ser tratada no controlador
            }
        }
    }
}