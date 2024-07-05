using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Creates
{
    public class StaffCreateModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
