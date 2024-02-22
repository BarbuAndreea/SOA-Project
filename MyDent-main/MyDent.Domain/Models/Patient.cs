using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyDent.Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }

        public User UserP { get; set; }

        public ICollection<Intervention> Interventions { get; set; }

        [JsonIgnore]
        public ICollection<Clinic> Clinics { get; set; }

        public ICollection<Radiography> Radiographies { get; set; }
    }
}
