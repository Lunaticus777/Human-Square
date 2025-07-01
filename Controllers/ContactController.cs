using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Human_Evolution.Controllers
{
    public class ContactController : Controller
    {
        private readonly SmtpSettings _smtpSettings;

        public ContactController(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.From),
                    Subject = "Message depuis le site Human Square",
                    Body = $"Nom: {model.Name}\nEmail: {model.Email}\nMessage:\n{model.Message}",
                    IsBodyHtml = false
                };

                mail.To.Add(_smtpSettings.To);

                using (var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password);
                    smtp.EnableSsl = _smtpSettings.EnableSsl;
                    await smtp.SendMailAsync(mail);
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500, "Erreur lors de l’envoi");
            }
        }
    }
}
