using MyDent.Domain.Enum;
using System;

namespace MyDent.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int MedicId { get; set; }

        public int RoomId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public AppointmentStatus Status { get; set; }

    }
}
