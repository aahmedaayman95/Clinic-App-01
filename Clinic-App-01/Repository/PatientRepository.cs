using Clinic_App_01.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_App_01.Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ClinicContext context) : base(context) { }

        public bool PatientExists(int? id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<Patient>> GetPatientsSortedByNameAsync()
        {
            return await _context.Patients.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsSortedByAgeAsync()
        {
            return await _context.Patients.OrderBy(p => p.Age).ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsSortedByMaleGenderAsync()
        {
            return await _context.Patients.Where(p => p.Gender == Gender.Male).ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsSortedByFemaleGenderAsync()
        {
            return await _context.Patients.Where(p => p.Gender == Gender.Female).ToListAsync();
        }
    }
}
