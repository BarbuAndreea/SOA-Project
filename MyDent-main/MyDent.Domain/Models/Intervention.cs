using MyDent.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyDent.Domain.Models
{
    public class Intervention
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int MedicId { get; set; }

        public int PatientId { get; set; }

        public ToothEnum Teeth { get; set; }
     
        public string Description { get; set; }

        public int Price { get; set; }

        public string Recommendation { get; set; }

        public DateTime Date { get; set; }

    }
}
