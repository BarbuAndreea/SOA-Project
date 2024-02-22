using MyDent.DataAccess;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services
{
    public class HolidayService : IHolidayService
    {

        private readonly MyDentDbContext _dbContext;

        public HolidayService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public List<Holiday> GetAllHolidaysByMedicId(int medicId)
        {
            return _dbContext.Holidays.Where(h => h.MedicId == medicId).OrderBy(h => h.StartDate).ToList();
        }

        public Holiday GetHolidayById(int id)
        {
            return _dbContext.Holidays.FirstOrDefault(x => x.Id == id);
        }

        public bool IsHolidayDateTimeValid(Holiday holiday)
        {
            if (holiday.StartDate.Date < DateTime.Today)
            {
                throw new ValidationException("You can't add a holiday at a past time.");
            }

            if (holiday.EndDate < holiday.StartDate)
            {
                throw new ValidationException("Invalid period of time! Ending date is earlier than starting date!\n");
            }
            else
            {
                var holidays = _dbContext.Holidays.Where(h => h.MedicId == holiday.MedicId).Where(b => (b.StartDate < holiday.StartDate && b.EndDate > holiday.StartDate) ||
                    (b.StartDate < holiday.EndDate && b.EndDate > holiday.EndDate) || (b.StartDate > holiday.StartDate && b.EndDate < holiday.EndDate)).ToList();
                if (holidays.Count > 0)
                {
                    throw new ValidationException("Invalid period of time! You already have an holiday for some of this days. Please check your holidays");
                }
            }
            return true;
        }

        public Holiday AddHoliday(Holiday holiday)
        {
            try
            {
                IsHolidayDateTimeValid(holiday);
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }

            _dbContext.Holidays.Add(holiday);
            _dbContext.SaveChanges();

            return holiday;
        }

        public Holiday UpdateHoliday(Holiday newHoliday)
        {
            IsHolidayDateTimeValid(newHoliday);
            
            var holiday = _dbContext.Holidays.FirstOrDefault(h => h.Id == newHoliday.Id);
            if (holiday != null)
            {
                holiday.StartDate = newHoliday.StartDate;
                holiday.EndDate = newHoliday.EndDate;
            }

            _dbContext.SaveChanges();

            return holiday;
        }

        public Holiday DeleteHolidayById(int id)
        {
            var holiday = _dbContext.Holidays.FirstOrDefault(a => a.Id == id);

            _dbContext.Holidays.Remove(holiday);
            _dbContext.SaveChanges();

            return holiday;
        }
    }
}
