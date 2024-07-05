using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Updates
{
    public class CustomerUpdateModel
    {
        public string Name { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
