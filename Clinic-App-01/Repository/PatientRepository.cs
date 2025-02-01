using Clinic_App_01.Models;

namespace Clinic_App_01.Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ClinicContext context) : base(context) { }

        public bool PatientExists(int? id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
