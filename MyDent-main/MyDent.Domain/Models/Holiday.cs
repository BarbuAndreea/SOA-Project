using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDent.Domain.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        public int MedicId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
