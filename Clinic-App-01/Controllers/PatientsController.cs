using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic_App_01.Models;
using Clinic_App_01.Repository;
using Microsoft.Data.SqlClient;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace Clinic_App_01.Controllers
{
    public class PatientsController : Controller
    {
        //private readonly ClinicContext _context;

        //public PatientsController(ClinicContext context)
        //{
        //    _context = context;
        //}

        private readonly IPatientRepository _patientRepository;
        private readonly HttpClient _httpClient;


        public PatientsController(IPatientRepository patientRepository , IHttpClientFactory httpClientFactory)
        {
            _patientRepository = patientRepository;
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder)
        {
            IEnumerable<Patient> patients = new List<Patient>();

            if (sortOrder == "name")
            {
                patients = await _patientRepository.GetPatientsSortedByNameAsync();
            }

            else if (sortOrder == "age")
            {
                patients = await _patientRepository.GetPatientsSortedByAgeAsync();
            }

            else if (sortOrder == "male")
            {
                patients = await _patientRepository.GetPatientsSortedByMaleGenderAsync();
            }

            else if (sortOrder == "female")
            {
                patients = await _patientRepository.GetPatientsSortedByFemaleGenderAsync();
            }
            else
            {
                //default order
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7107/api/patients");

                if (response.IsSuccessStatusCode)
                {


                    //to view data
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if(responseContent != null)
                    patients = JsonConvert.DeserializeObject<IEnumerable<Patient>>(responseContent);
                    

                }
                else
                {
                    // Handle error

                    return BadRequest();
                }
            }

            return View(patients);
            

        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Gender,ContactInfo")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                await _patientRepository.AddAsync(patient);
                await _patientRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Gender,ContactInfo")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _patientRepository.UpdateAsync(patient);
                    await _patientRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_patientRepository.PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient != null)
            {
                await _patientRepository.DeleteAsync(id);
            }

            await _patientRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int? id)
        {
            return _patientRepository.PatientExists(id);
        }
    }
}
