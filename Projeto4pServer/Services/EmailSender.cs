using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Amazon.Extensions.NETCore.Setup;

namespace Projeto4pServer.Services
{
    public class EmailService
    {
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly ILogger<EmailService> _logger;
        private readonly IAmazonSimpleEmailService _sesClient;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");
            _fromEmail = smtpSettings["FromEmail"]!;
            _fromName = smtpSettings["FromName"]!;
            _logger = logger;

            var awsOptions = configuration.GetAWSOptions();
            _sesClient = awsOptions.CreateServiceClient<IAmazonSimpleEmailService>();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = $"{_fromName} <{_fromEmail}>",
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { toEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content(body)
                        }
                    }
                };

                var response = await _sesClient.SendEmailAsync(sendRequest);
                _logger.LogInformation("E-mail enviado com sucesso para {toEmail}.", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail para {toEmail}.", toEmail);
                throw; // Re-lança a exceção para ser tratada no controlador
            }
        }
    }
}