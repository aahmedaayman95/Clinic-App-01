using System.ComponentModel.DataAnnotations;

namespace Clinic_App_01.Models
{
    public class Patient
    {

        [Key]
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public Gender Gender { get; set; } 
        public string ContactInfo { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
