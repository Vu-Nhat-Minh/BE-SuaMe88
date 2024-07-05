using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Filters
{
    public class StaffFilterModel
    {
        public string? Username { get; set; } = null!;

        public string? Name { get; set; } = null!;

        public string? Status { get; set; } = null!;
        //public DateTime? From { get; set; }
        //public DateTime? To { get; set; }
    }
}
