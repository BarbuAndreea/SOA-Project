using MyDent.Domain.DTO;
using MyDent.Domain.Models;
using System.Collections.Generic;

namespace MyDent.Services.Abstractions
{
    public interface IClinicService
    {
        List<Clinic> GetAllClinics();

        Clinic GetClinicById(int id);

        Clinic AddClinic(ClinicDto newClinic);

        Clinic GetClinicByMedic(Medic medic);

        Clinic DeleteClinicById(int id);

        Clinic UpdateClinic(ClinicDto clinic);

        List<Clinic> GetAllClinicsByPatient(Patient patient);
    }
}
