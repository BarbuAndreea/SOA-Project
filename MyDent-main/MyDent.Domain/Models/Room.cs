using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyDent.Domain.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string MedicalEquipment { get; set; }

        [Required]
        public int ClinicId { get; set; }

        public ICollection<Appointment>  Appointments { get; set; }

    }
}
