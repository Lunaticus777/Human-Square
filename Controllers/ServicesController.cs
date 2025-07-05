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
        public IActionResult Index()
        {
            return View();
        }

        // ✅ Traitement du formulaire Orientation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitServiceRequest(ServiceRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Veuillez remplir tous les champs correctement.";
                return View("Orientation", model);
            }

            try
            {
                // Sauvegarde en base
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

                // Message email
                string subject = "Demande via Orientation – Human Square";

                string body = $@"
                    <h2>Nouvelle demande de service (Orientation)</h2>
                    <p><strong>Nom :</strong> {model.Name}</p>
                    <p><strong>Email :</strong> {model.Email}</p>
                    <p><strong>Téléphone :</strong> {model.Phone}</p>
                    <p><strong>Domaines sélectionnés :</strong> {model.SelectedDomains}</p>
                    <p><strong>Prestations choisies :</strong> {model.ServiceType}</p>
                    <p><strong>Message :</strong><br>{model.Message}</p>";

                await _mailService.SendEmailAsync(subject, body, model.Email);

                TempData["SuccessMessage"] = "Votre demande a bien été envoyée.";
                return RedirectToAction("Orientation");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de l'envoi : " + ex.Message;
                return RedirectToAction("Orientation", model);
            }
        }
    }
}
