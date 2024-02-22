using MyDent.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDent.Domain.Models
{
    public class Medic
    {
        public int Id { get; set; }

        public User UserM { get; set; }

        public SpecializationEnum Specialization { get; set; }

        public int ClinicId { get; set; }

        public DateTime StartWorkingHour { get; set; }

        public DateTime EndWorkingHour { get; set; }

        public ICollection<Holiday> Holidays { get; set; }
    }
}
