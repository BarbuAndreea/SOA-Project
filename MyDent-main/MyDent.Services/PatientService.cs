using Microsoft.EntityFrameworkCore;
using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace MyDent.Services
{
    public class PatientService : IPatientService
    {
        private readonly MyDentDbContext _dbContext;

        public PatientService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public Patient AddPatient(User newUser)
        {
            Patient patient = new()
            {
                UserP = newUser,
                Interventions = new List<Intervention>()
            };

            _dbContext.Patients.Add(patient);
            _dbContext.SaveChanges();

            return patient;
        }

        public Patient GetPatientById(int id)
        {
            return _dbContext.Patients.Include(p => p.UserP).Include(p => p.Interventions).Include(p => p.Radiographies).FirstOrDefault(x => x.Id == id);
        }

        public Patient GetPatientByUserId(int id)
        {
            return _dbContext.Patients.Include(p => p.UserP).Include(p => p.Clinics).Include(p => p.Interventions).Include(p => p.Radiographies).FirstOrDefault(x => x.UserP.Id == id);
        }

        public List<Patient> GetAllPatientsByClinic(Clinic clinic)
        {
            var patients = _dbContext.Patients.Include(p => p.UserP).Include(p => p.Clinics).Where(p => p.Clinics.Contains(clinic)).OrderBy(p => p.UserP.LastName).ToList();
            if (patients == null)
                return null;
            return patients;
        }

        public List<Patient> GetPatientsByName(string firstName, string lastName, Clinic clinic)
        {
            List<Patient> patients = new List<Patient>();
            if (firstName == "null" || lastName == "null")
            {
                patients = _dbContext.Patients.Include(p => p.UserP).Include(p => p.Clinics).Where(p => p.Clinics.Contains(clinic) && (p.UserP.FirstName == firstName || p.UserP.LastName == lastName)).ToList();
            }
            else
            {
                patients = _dbContext.Patients.Include(p => p.UserP).Include(p => p.Clinics).Where(p => p.Clinics.Contains(clinic) && (p.UserP.FirstName == firstName && p.UserP.LastName == lastName)).ToList();
            }

            if (patients.Count == 0)
            {
                throw new UserException($"No patient with given name was found", 400);
            }
            return patients;
        }

        public Patient DeletePatientByUserId(int userId)
        {
            var patientByUserId = _dbContext.Patients.Include(u => u.UserP).SingleOrDefault(p => p.UserP.Id == userId);

            if (patientByUserId == null)
            {
                throw new UserException($"Patient was not found", 404);
            }

            _dbContext.Patients.Remove(patientByUserId);
            _dbContext.SaveChanges();

            return patientByUserId;
        }

        public async Task<Patient> AddClinicToPatient(string patientId, Clinic clinic)
        {
            var patient = _dbContext.Patients.Include(u => u.Clinics).Include(u=>u.UserP).FirstOrDefault(p => p.UserP.PersonalCode == patientId);

            if (patient.Clinics == null)
            {
                patient.Clinics = new List<Clinic>();
            }

            if (patient.Clinics.Contains(clinic))
            {
                throw new Exception("Already exist!");
            }
            else
            {
                patient.Clinics.Add(clinic);
                await _dbContext.SaveChangesAsync();
            }
            return patient;
        }

        public void UpdateRadiograpyPatient(Patient patient)
        {
            _dbContext.Patients.Update(patient);
        }
    }
}
