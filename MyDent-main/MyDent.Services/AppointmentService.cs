using MyDent.DataAccess;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly MyDentDbContext _dbContext;

        public AppointmentService(MyDentDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public Appointment AddAppointment(Appointment newAppointment)
        {
            try
            {
                IsAppointmentDateTimeValid(newAppointment);
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }

            _dbContext.Appointments.Add(newAppointment);
            _dbContext.SaveChanges();

            return newAppointment;
        }


        public Appointment UpdateAppointment(Appointment newAppointment)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(a => a.Id == newAppointment.Id);
            if (appointment.PatientId == newAppointment.PatientId && appointment.MedicId == newAppointment.MedicId && appointment.RoomId == newAppointment.RoomId &&
                appointment.StartDate == newAppointment.StartDate && appointment.EndDate == newAppointment.EndDate)
            {
                appointment.Status = newAppointment.Status;
                appointment.Name = newAppointment.Name;
                _dbContext.SaveChanges();
            }
            else
            {
                try
                {
                    IsAppointmentDateTimeValid(newAppointment);
                    appointment.PatientId = newAppointment.PatientId;
                    appointment.MedicId = newAppointment.MedicId;
                    appointment.RoomId = newAppointment.RoomId;
                    appointment.StartDate = newAppointment.StartDate;
                    appointment.EndDate = newAppointment.EndDate;
                    appointment.Status = newAppointment.Status;
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return newAppointment;
        }

        public Appointment UpdateAppointmentStatus(Appointment appointment)
        {
            _dbContext.Appointments.Update(appointment);

            _dbContext.SaveChanges();

            return appointment;
        }

        public List<Appointment> GetTodayAppointments()
        {
            var date = DateTime.Now;
            return _dbContext.Appointments.Where(a => a.StartDate.Date== date.Date).OrderBy(h => h.StartDate).ToList();
        }

        public List<Appointment> GetAppointmentsByDateAndMedicId(DateTime date, int medicId)
        {
            var appointments = _dbContext.Appointments.Where(a => a.StartDate.Date == date.Date && a.MedicId == medicId).OrderBy(a => a.StartDate).ToList();
            if (appointments.Count == 0)
            {
                throw new ValidationException("There is no appointments for this day.");
            }
            return appointments;
        }

        public List<Appointment> GetAppointmentsByDateAndClinicId(DateTime date, int clinicId)
        {
            var medics = _dbContext.Medics.Where(m => m.ClinicId==clinicId).ToList();
            var clinicAppointments = new List<Appointment>();
            foreach (var m in medics)
            {
                var appointments = _dbContext.Appointments.Where(a => a.StartDate.Date == date.Date && a.MedicId==m.Id).OrderBy(a => a.StartDate).ToList();
                clinicAppointments.AddRange(appointments);
            }
            if (clinicAppointments.Count == 0)
            {
                throw new ValidationException("There is no appointments for this day.");
            }
            return clinicAppointments;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            var appointments = _dbContext.Appointments.Where(a => a.StartDate.Date == date.Date).ToList();
            if (appointments.Count == 0)
            {
                throw new ValidationException("There is no appointments for this day.");
            }
            return appointments;
        }

        public bool IsAppointmentDateTimeValid(Appointment appointment)
        {
            if (appointment.StartDate.DayOfWeek == DayOfWeek.Saturday || appointment.StartDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new ValidationException("Closed during the weekend!\n");
            }

            if (appointment.StartDate < DateTime.Now)
            {
                throw new ValidationException("You can't make an appoinment at a past time.");
            }

            if (appointment.EndDate < appointment.StartDate)
            {
                throw new ValidationException("Invalid period of time! Ending time is earlier than starting time!\n");
            }
            else
            {
                TimeSpan appointmentPeriod = appointment.EndDate - appointment.StartDate;
                if (appointmentPeriod.TotalMinutes < 10)
                {
                    throw new ValidationException("Invalid period of time! The minimum time period is 10 minute!\n");
                }
                else
                {
                    var appointmentRoom = _dbContext.Appointments.Where(b => ((b.StartDate < appointment.StartDate && b.EndDate > appointment.StartDate) ||
                (b.StartDate < appointment.EndDate && b.EndDate > appointment.EndDate) || (b.StartDate > appointment.StartDate && b.EndDate < appointment.EndDate))
                && b.RoomId == appointment.RoomId && b.Id != appointment.Id).ToList();
                    if (appointmentRoom.Count > 0)
                    {
                        throw new ValidationException("Room is occupied at the chosen time.");
                    }
                    var holidaysAppointment = _dbContext.Holidays.Where(h => h.MedicId == appointment.MedicId && (h.StartDate.Date <= appointment.StartDate.Date && appointment.StartDate.Date <= h.EndDate.Date)).ToList();
                    if (holidaysAppointment.Count > 0)
                    {
                        throw new ValidationException("There is a holiday for this date.");
                    }
                    var m = _dbContext.Medics.Find(appointment.MedicId);
                    if (!(appointment.StartDate.TimeOfDay > m.StartWorkingHour.TimeOfDay && appointment.EndDate.TimeOfDay < m.EndWorkingHour.TimeOfDay))
                    {
                        throw new ValidationException("Ouside the schedule.");
                    }
                }
                return true;
            }
        }

        public Appointment DeleteAppointmentByPatientIdAndDate(int patientId)
        {
            var appointmentByPatientId = _dbContext.Appointments.SingleOrDefault(p => p.PatientId == patientId);

            if (appointmentByPatientId == null)
            {
                throw new UserException($"Patient was not found", 404);
            }

            _dbContext.Appointments.Remove(appointmentByPatientId);
            _dbContext.SaveChanges();

            return appointmentByPatientId;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _dbContext.Appointments.ToList();
        }

        public List<Appointment> GetMedicAppointmentsByStatus(AppointmentStatus status, int medicId)
        {
            var x = _dbContext.Appointments.Where(a => a.MedicId == medicId && a.Status == status).OrderBy(a => a.StartDate).ToList();
            if (x.Count == 0)
            {
                throw new Exception("No active appointments!");
            }
            return x;
        }

        public List<Appointment> GetNextAppointmentsByPatientId(int patientId)
        {
            return _dbContext.Appointments.Where(x => x.PatientId == patientId && DateTime.Now < x.StartDate).OrderByDescending(a => a.StartDate).ToList();
        }

        public List<Appointment> GetPastAppointmentsByPatientId(int patientId)
        {
            return _dbContext.Appointments.Where(x => x.PatientId == patientId && DateTime.Now > x.EndDate).OrderByDescending(a => a.StartDate).ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            return _dbContext.Appointments.FirstOrDefault(x => x.Id == id);
        }

        public Appointment DeleteAppointmentById(int id)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                throw new NullReferenceException("Appointment with the given id was not found");
            }

            _dbContext.Appointments.Remove(appointment);
            _dbContext.SaveChanges();

            return appointment;
        }
    }
}
