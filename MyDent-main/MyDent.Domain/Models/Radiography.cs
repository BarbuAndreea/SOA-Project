using System;

namespace MyDent.Domain.Models
{
    public class Radiography
    {
        public int Id { get; set; }

        public string Image64 { get; set; }

        public DateTime Date { get; set; }
    }
}
