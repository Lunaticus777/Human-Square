using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Human_Evolution.Data;

namespace Human_Evolution.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Orientation()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitServiceRequest(ServiceRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new ServiceRequest
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    ServiceType = model.ServiceType,
                    Message = model.Message
                };

                _context.ServiceRequests.Add(request);
                _context.SaveChanges();

                TempData["Success"] = "Votre demande a bien été envoyée. Nous vous recontacterons rapidement.";
                return RedirectToAction("Orientation");
            }

            TempData["Error"] = "Une erreur est survenue lors de l'envoi du formulaire.";
            return View("Orientation", model);
        }
    }
}
