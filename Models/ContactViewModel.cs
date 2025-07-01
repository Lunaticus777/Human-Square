using System.ComponentModel.DataAnnotations;

namespace Human_Evolution.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Le nom est requis.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Adresse email invalide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le message est requis.")]
        public string Message { get; set; }
    }
}

