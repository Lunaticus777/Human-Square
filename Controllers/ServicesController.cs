using Human_Evolution.Data;
using Human_Evolution.Models;
using Human_Evolution.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Human_Evolution.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MailService _mailService;

        public ServicesController(ApplicationDbContext context, MailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        // Vue Orientation
        public IActionResult Orientation()
        {
            return View(new ServiceRequestViewModel());
        }

        // Vue Services complète
        public IActionResult Services()
        {
            return View();
        }

        // ✅ Traitement du formulaire
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitServiceRequest(ServiceRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new ServiceRequest
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    ServiceType = model.ServiceType,
                    Message = model.Message,
                    SelectedDomains = model.SelectedDomains
                };

                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                var emailBody = $@"
                    <h2>Nouvelle demande de service (Orientation)</h2>
                    <p><strong>Nom :</strong> {model.Name}</p>
                    <p><strong>Email :</strong> {model.Email}</p>
                    <p><strong>Téléphone :</strong> {model.Phone}</p>
                    <p><strong>Domaines :</strong> {model.SelectedDomains}</p>
                    <p><strong>Prestations :</strong> {model.ServiceType}</p>
                    <p><strong>Message :</strong><br>{model.Message}</p>";

                await _mailService.SendEmailAsync(
                    "Nouvelle demande via Orientation",
                    emailBody,
                    replyTo: model.Email
                );

                TempData["SuccessMessage"] = "Votre demande a bien été envoyée.";
                return RedirectToAction("Orientation");
            }

            TempData["ErrorMessage"] = "Une erreur est survenue. Merci de vérifier les champs.";
            return View("Orientation", model);
        }
    }
}
