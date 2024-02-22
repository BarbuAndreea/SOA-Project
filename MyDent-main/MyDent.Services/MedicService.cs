using Microsoft.EntityFrameworkCore;
using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDent.Services
{
    public class MedicService : IMedicService
    {
        private readonly MyDentDbContext _dbContext;

        public MedicService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public Medic AddMedic(Medic medic)
        {
            _dbContext.Medics.Add(medic);
            _dbContext.SaveChanges();

            return medic;
        }

        public async Task UpdateMedic(Medic updatedMedic)
        {
            var medicById = _dbContext.Medics.FirstOrDefault(u => u.Id == updatedMedic.Id);
            if (medicById != null)
            {
                medicById.StartWorkingHour = updatedMedic.StartWorkingHour;
                medicById.EndWorkingHour = updatedMedic.EndWorkingHour;
            }

            await _dbContext.SaveChangesAsync();
        }

        public Medic DeleteMedicByUserId(int userId)
        {
            var medicByUserId = _dbContext.Medics.SingleOrDefault(p => p.UserM.Id == userId);

            if (medicByUserId == null)
            {
                throw new UserException($"Patient was not found", 404);
            }

            _dbContext.Medics.Remove(medicByUserId);
            _dbContext.SaveChanges();

            return medicByUserId;
        }

        public List<Medic> GetAllMedics()
        {
            return _dbContext.Medics.Include(m => m.UserM).ToList();
        }

        public List<Medic> GetMedicsByClinic(int clinicId)
        {
            return _dbContext.Medics.Include(m => m.UserM).Where(c => c.ClinicId == clinicId).ToList();
        }

        public Medic GetMedicByUserId(int userId)
        {
            return _dbContext.Medics.Include(m => m.UserM).Include(h => h.Holidays).FirstOrDefault(x => x.UserM.Id == userId);
        }

        public Medic GetMedicById(int medicId)
        {
            return _dbContext.Medics.Include(m => m.UserM).FirstOrDefault(x => x.Id == medicId);
        }
    }
}
