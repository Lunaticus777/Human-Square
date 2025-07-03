using Human_Evolution.Models;
using Human_Evolution.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Human_Evolution.Controllers
{
    public class ContactController : Controller
    {
        private readonly MailService _mailService;

        public ContactController(MailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactViewModel()); // charge /Views/Contact/Contact.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Veuillez remplir tous les champs correctement.";
                return View("Contact", model);
            }

            try
            {
                string subject = "Message depuis le site Human Square";
                string body = $"<strong>Nom :</strong> {model.Name}<br/>" +
                              $"<strong>Email :</strong> {model.Email}<br/>" +
                              $"<strong>Message :</strong><br/>{model.Message}";

                await _mailService.SendEmailAsync(subject, body, model.Email);

                TempData["SuccessMessage"] = "Votre message a été envoyé avec succès.";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de l'envoi : " + ex.Message;
                return RedirectToAction("Contact");
            }
        }
    }
}
