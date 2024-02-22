using Microsoft.EntityFrameworkCore;
using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services
{
    public class ClinicAdminService : IClinicAdminService
    {
        private readonly MyDentDbContext _dbContext;

        public ClinicAdminService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public ClinicAdmin AddClinicAdmin(ClinicAdmin clinicAdmin)
        {

            _dbContext.ClinicAdmins.Add(clinicAdmin);
            _dbContext.SaveChanges();

            return clinicAdmin;
        }

        public ClinicAdmin GetClinicAdminByUserId(int userId)
        {
            return _dbContext.ClinicAdmins.Include(m => m.UserC).FirstOrDefault(x => x.UserC.Id == userId);
        }
    }
}
