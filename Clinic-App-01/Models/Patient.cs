using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace Clinic_App_01.Models
{
    public class Patient
    {

        [Key]
        public int Id { get; set; } // Primary Key

        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your age")]
        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please select your gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Please enter your contact information")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Enter a valid phone number (10-15 digits)")]
        public string ContactInfo { get; set; } = string.Empty;
        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
