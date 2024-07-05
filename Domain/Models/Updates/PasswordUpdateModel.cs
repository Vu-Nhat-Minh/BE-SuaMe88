using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Updates
{
    public class PasswordUpdateModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
