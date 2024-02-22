using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Domain.Models
{
    public class ClinicAdmin
    {
        public int Id { get; set; }

        public User UserC { get; set; }

        public int ClinicId { get; set; }
    }
}
