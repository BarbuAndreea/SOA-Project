using MyDent.Domain.Models;
using System.Collections.Generic;

namespace MyDent.Services.Abstractions
{
    public interface IInterventionService
    {
        List<Intervention> GetAllInterventions();

        Intervention GetInterventionById(int id);

        List<Intervention> GetInterventionByPatient(int patientId);

        Intervention AddNewIntervention(Intervention newIntervention);

        Intervention UpdateIntervention(Intervention intervention);

    }
}
