using MyDent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IClinicAdminService
    {
        ClinicAdmin GetClinicAdminByUserId(int userId);
        ClinicAdmin AddClinicAdmin(ClinicAdmin clinicAdmin);
    }
}
