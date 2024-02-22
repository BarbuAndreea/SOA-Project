using MyDent.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IPatientService
    {
        Patient AddPatient(User newUser);

        List<Patient> GetAllPatientsByClinic(Clinic clinic);

        Patient GetPatientById(int id);

        Patient GetPatientByUserId(int id);

        List<Patient> GetPatientsByName(string firstName, string lastName, Clinic clinic);

        Patient DeletePatientByUserId(int userId);

        Task<Patient> AddClinicToPatient(string patientId, Clinic clinic);

        void UpdateRadiograpyPatient(Patient patient);
    }
}
