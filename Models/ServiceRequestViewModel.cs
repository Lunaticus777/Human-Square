using System.ComponentModel.DataAnnotations;

namespace Human_Evolution.Models
{
    public class ServiceRequestViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [Display(Name = "Nom complet")]
        public string Name { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Le téléphone n'est pas valide.")]
        [Display(Name = "Téléphone")]
        public string Phone { get; set; }

        [Display(Name = "Votre besoin")]
        public string ServiceType { get; set; } // ← utilisé pour stocker les prestations sélectionnées

        [Display(Name = "Message complémentaire")]
        public string Message { get; set; }

        // ✅ Ajoute ce champ pour que le formulaire puisse le binder
        [Display(Name = "Domaines sélectionnés")]
        public string SelectedDomains { get; set; }
    }
}
