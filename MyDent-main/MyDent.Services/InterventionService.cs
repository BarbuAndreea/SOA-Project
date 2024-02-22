using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services
{
    public class InterventionService : IInterventionService
    {
        private readonly MyDentDbContext _dbContext;

        public InterventionService(MyDentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Intervention> GetAllInterventions()
        {
            return _dbContext.Interventions.ToList();
        }

        public Intervention GetInterventionById(int id)
        {
            return _dbContext.Interventions.FirstOrDefault(i => i.Id == id);
        }

        public List<Intervention> GetInterventionByPatient(int patientId)
        {
            return _dbContext.Interventions.Where(i => i.PatientId == patientId).OrderByDescending(i => i.Date).ToList();
        }

        public Intervention UpdateIntervention(Intervention newIntervention)
        {
            var intervention = _dbContext.Interventions.FirstOrDefault(i => i.Id == newIntervention.Id);
            intervention.Name = newIntervention.Name;
            intervention.Description = newIntervention.Description;
            intervention.Date = newIntervention.Date;
            intervention.Price = newIntervention.Price;
            intervention.Teeth = newIntervention.Teeth;
            intervention.Recommendation = newIntervention.Recommendation;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new DatabaseException("Could not save changes after update.");
            }

            return newIntervention;
        }

        public Intervention AddNewIntervention(Intervention newIntervention)
        {
            _dbContext.Interventions.Add(newIntervention);
            _dbContext.SaveChanges();

            return newIntervention;
        }
    }
}
