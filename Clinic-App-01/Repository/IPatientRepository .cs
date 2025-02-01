using Clinic_App_01.Models;

namespace Clinic_App_01.Repository
{
    public interface IPatientRepository : IRepository<Patient>
    {
        // You can add extra methods specific to Patients here

         bool PatientExists(int? id);

        Task<IEnumerable<Patient>> GetPatientsSortedByNameAsync();
        Task<IEnumerable<Patient>> GetPatientsSortedByAgeAsync();

        Task<IEnumerable<Patient>> GetPatientsSortedByMaleGenderAsync();

        Task<IEnumerable<Patient>> GetPatientsSortedByFemaleGenderAsync();
    }

}
