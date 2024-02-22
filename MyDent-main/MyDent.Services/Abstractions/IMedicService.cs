using MyDent.Domain.DTO;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IMedicService
    {
        Medic AddMedic(Medic medic);

        List<Medic> GetAllMedics();

        Medic DeleteMedicByUserId(int userId);

        Medic GetMedicByUserId(int userId);

        Medic GetMedicById(int medicId);

        Task UpdateMedic(Medic newMedic);

        List<Medic> GetMedicsByClinic(int clinicId);
    }
}
