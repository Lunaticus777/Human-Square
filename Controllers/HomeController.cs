using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Human_Evolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SmtpSettings _smtpSettings;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpSettings)
        {
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
        }

        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Services() => View();

        public IActionResult Projects() => View();

        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.From),
                        Subject = "Nouveau message via le formulaire de contact",
                        Body = $"Nom : {model.Name}\nEmail : {model.Email}\n\nMessage :\n{model.Message}",
                        IsBodyHtml = false
                    };

                    mail.To.Add(_smtpSettings.To);

                    using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                    {
                        Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password),
                        EnableSsl = _smtpSettings.EnableSsl
                    };

                    smtp.Send(mail);

                    // ✅ Message de succès
                    TempData["SuccessMessage"] = "Message envoyé avec succès.";
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    // ❌ Message d'erreur
                    TempData["ErrorMessage"] = $"Erreur lors de l'envoi : {ex.Message}";
                    return RedirectToAction("Contact");
                }
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ServicesContact(ServicesContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = $"Nom: {model.FullName}\nEmail: {model.Email}\n\nDomaines sélectionnés:\n{model.SelectedDomains}\n\nServices sélectionnés:\n{model.SelectedServices}\n\nMessage:\n{model.Message}";

                    var mail = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.From),
                        Subject = "Demande via Services & Formations",
                        Body = body,
                        IsBodyHtml = false
                    };
                    mail.To.Add(_smtpSettings.To);

                    using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                    {
                        Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password),
                        EnableSsl = _smtpSettings.EnableSsl
                    };

                    smtp.Send(mail);
                    TempData["SuccessMessage"] = "Votre message a bien été envoyé.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Erreur lors de l'envoi : {ex.Message}";
                }
            }

            return RedirectToAction("Services");
        }

    }
}
