using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyDent.Domain.Models
{
    public class Clinic
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string MapsAddress { get; set; }

        public ICollection<Medic> Medics { get; set; }

        public ICollection<Room> Rooms { get; set; }

        [JsonIgnore]
        public ICollection<Patient> Patients { get; set; }
    }
}
