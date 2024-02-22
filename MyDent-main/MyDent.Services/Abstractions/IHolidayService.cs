using MyDent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IHolidayService
    {
        List<Holiday> GetAllHolidaysByMedicId(int medicId);

        Holiday GetHolidayById(int id);

        Holiday AddHoliday(Holiday holiday);

        Holiday UpdateHoliday(Holiday holiday);

        Holiday DeleteHolidayById(int id);
    }
}
