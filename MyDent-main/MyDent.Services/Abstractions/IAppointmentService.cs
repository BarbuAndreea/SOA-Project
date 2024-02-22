using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyDent.Services.Abstractions
{
    public interface IAppointmentService
    {
        Appointment AddAppointment(Appointment newAppointment);

        Appointment DeleteAppointmentByPatientIdAndDate(int patientId);

        List<Appointment> GetAppointmentsByDate(DateTime date);
        Appointment UpdateAppointment(Appointment newAppointment);

        Appointment UpdateAppointmentStatus(Appointment appointment);

        List<Appointment> GetAllAppointments();

        List<Appointment> GetMedicAppointmentsByStatus(AppointmentStatus status, int medicId);

        List<Appointment> GetNextAppointmentsByPatientId(int patientId);

        List<Appointment> GetPastAppointmentsByPatientId(int patientId);

        Appointment GetAppointmentById(int Id);

        List<Appointment> GetTodayAppointments();

        Appointment DeleteAppointmentById(int id);
        List<Appointment> GetAppointmentsByDateAndMedicId(DateTime date, int medicId);
        List<Appointment> GetAppointmentsByDateAndClinicId(DateTime date, int clinicId);
    }
}
