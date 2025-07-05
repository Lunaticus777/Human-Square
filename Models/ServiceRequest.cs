using System;
using System.ComponentModel.DataAnnotations;

namespace Human_Evolution.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string ServiceType { get; set; }
        public string SelectedDomains { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
