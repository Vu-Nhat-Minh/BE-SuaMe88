using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Filters
{
    public class VoucherFilterModel
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public int? MinOrderValue { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
