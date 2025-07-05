using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Human_Evolution.Services
{
    public class MailService
    {
        private readonly SmtpSettings _settings;

        public MailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string subject, string body, string replyTo = null)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Human Square", _settings.From));
            message.To.Add(MailboxAddress.Parse("geralvelho@gmail.com")); // ← Tu reçois le test
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = builder.ToMessageBody();

            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                message.ReplyTo.Add(new MailboxAddress(replyTo, replyTo));
            }

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.User, _settings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                throw new Exception("Échec de l'authentification SMTP : vérifiez le login et le mot de passe.", ex);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Erreur lors de l'envoi du message : " + ex.Message, ex);
            }
        }
    }
}
