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
using Azure;
using System.Text;

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
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7107/Api/Patients/SortByName?name={sortOrder}");


                //this code need to be refactored as it repeated many places.
                if (response.IsSuccessStatusCode)
                {


                    //to view data
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent != null)
                        patients = JsonConvert.DeserializeObject<IEnumerable<Patient>>(responseContent);


                }
                else
                {
                    // Handle error

                    return BadRequest();
                }
            }

            else if (sortOrder == "age")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7107/Api/Patients/SortByAge?name={sortOrder}");


                //this code need to be refactored as it repeated many places.
                if (response.IsSuccessStatusCode)
                {


                    //to view data
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent != null)
                        patients = JsonConvert.DeserializeObject<IEnumerable<Patient>>(responseContent);


                }
                else
                {
                    // Handle error

                    return BadRequest();
                }
            }

            else if (sortOrder == "male")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7107/Api/Patients/ShowMales?name={sortOrder}");


                //this code need to be refactored as it repeated many places.
                if (response.IsSuccessStatusCode)
                {


                    //to view data
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent != null)
                        patients = JsonConvert.DeserializeObject<IEnumerable<Patient>>(responseContent);


                }
                else
                {
                    // Handle error

                    return BadRequest();
                }
            }

            else if (sortOrder == "female")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7107/Api/Patients/ShowFemales?name={sortOrder}");


                //this code need to be refactored as it repeated many places.
                if (response.IsSuccessStatusCode)
                {


                    //to view data
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent != null)
                        patients = JsonConvert.DeserializeObject<IEnumerable<Patient>>(responseContent);


                }
                else
                {
                    // Handle error

                    return BadRequest();
                }
            }
            else
            {
                //default order
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7107/api/patients");


                //this code need to be refactored as it repeated many places.
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

            Patient patient = new Patient(); 


            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7107/Api/Patients/{id}");

            //this code need to be refactored as it repeated many places.
            if (response.IsSuccessStatusCode)
            {


                //to view data
                string responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent != null)
                    patient = JsonConvert.DeserializeObject<Patient>(responseContent);


            }
            else
            {
                // Handle error

                return BadRequest();
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

            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                string apiUrl = "https://localhost:7107/api/patients";

                // Convert patient object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");

                // Send POST request to API
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to the list after successful creation
                }
                else
                {
                    ModelState.AddModelError("", "Error occurred while creating the patient.");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
            }

            return View(patient); // Return to the form in case of failure


            //Using Repository pattern not apis
            //if (ModelState.IsValid)
            //{
            //    await _patientRepository.AddAsync(patient);
            //    await _patientRepository.SaveAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(patient);
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

            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                // Convert patient object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");

                // Send PUT request to API
                HttpResponseMessage response = await _httpClient.PutAsync($"https://localhost:7107/api/patients", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(); // Patient not found
                }
                else
                {
                    ModelState.AddModelError("", "Error occurred while updating the patient.");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
            }

            return View(patient); // Return to the form in case of failure


            //Old Code Using Repo Pattern not API
            //if (id != patient.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        await _patientRepository.UpdateAsync(patient);
            //        await _patientRepository.SaveAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!_patientRepository.PatientExists(patient.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient? patient = null;

            try
            {
                string apiUrl = $"https://localhost:7107/api/patients/{id}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        patient = JsonConvert.DeserializeObject<Patient>(responseContent);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Error retrieving patient data.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
            }

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);


            //Old Code Using Repo Pattern
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var patient = await _patientRepository.GetByIdAsync(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}

            //return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                string apiUrl = $"https://localhost:7107/api/patients/{id}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Error occurred while deleting the patient.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));

            //Old COde Using Repo Pattern
            //var patient = await _patientRepository.GetByIdAsync(id);

            //if (patient != null)
            //{
            //    await _patientRepository.DeleteAsync(id);
            //}

            //await _patientRepository.SaveAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int? id)
        {
            return _patientRepository.PatientExists(id);
        }
    }
}
