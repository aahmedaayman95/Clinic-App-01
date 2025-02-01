using System.ComponentModel.DataAnnotations;

namespace Clinic_App_01.Models
{
    public class Appointment
    {

        [Key]
        public int Id { get; set; } // Primary Key

        // Foreign Keys
        public int PatientId { get; set; }
        public Patient Patient { get; set; } // Navigation Property

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } // Navigation Property

        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; }
    }
}
