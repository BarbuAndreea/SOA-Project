using Microsoft.EntityFrameworkCore;
using MyDent.DataAccess;
using MyDent.Domain.DTO;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services
{

    public class ClinicService : IClinicService
    {
        private readonly MyDentDbContext _dbContext;

        public ClinicService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public List<Clinic> GetAllClinics()
        {
            return _dbContext.Clinics.ToList();
        }

        public Clinic GetClinicById(int id)
        {
            return _dbContext.Clinics.FirstOrDefault(x => x.Id == id);
        }

        public Clinic GetClinicByMedic(Medic medic)
        {
            return _dbContext.Clinics.FirstOrDefault(c => c.Medics.Contains(medic));
        }

        public Clinic AddClinic(ClinicDto clinic)
        {
            var newClinic = new Clinic
            {
                Address = clinic.Address,
                Description = clinic.Description,
                Email = clinic.Email,
                Name = clinic.Name,
                PhoneNumber = clinic.PhoneNumber,
                Medics=new List<Medic>(),
                Patients= new List<Patient>(),
                Rooms=new List<Room>(),
                MapsAddress = clinic.MapsAddress
            };

            try
            {
                _dbContext.Clinics.Add(newClinic);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new DatabaseException("Could not save changes after add.");
            }

            return newClinic;
        }

        public Clinic DeleteClinicById(int id)
        {
            var clinic = _dbContext.Clinics.FirstOrDefault(a => a.Id == id);

            if (clinic == null)
            {
                throw new NullReferenceException("Appointment with the given id was not found");
            }

            _dbContext.Clinics.Remove(clinic);
            _dbContext.SaveChanges();

            return clinic;
        }

        public Clinic UpdateClinic(ClinicDto clinic)
        {
            var clinicToUpdate = _dbContext.Clinics.Include(r => r.Patients).Include(r => r.Medics).Include(r => r.Rooms).FirstOrDefault(r => r.Id == clinic.Id);
            clinicToUpdate.Name = clinic.Name;
            clinicToUpdate.Address=clinic.Address;
            clinicToUpdate.PhoneNumber= clinic.PhoneNumber;
            clinicToUpdate.Description= clinic.Description;
            clinicToUpdate.Email= clinic.Email;
            
            _dbContext.Clinics.Update(clinicToUpdate);
            _dbContext.SaveChanges();

            return clinicToUpdate;
        }

        public List<Clinic> GetAllClinicsByPatient(Patient patient)
        {
            var clinics = _dbContext.Clinics.Where(p => p.Patients.Contains(patient)).ToList();
            if (clinics.Count == 0)
                return null;
            return clinics;
        }
    }
}
