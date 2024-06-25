using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Filters
{
    public class OrderFilterModel
    {
        public string? Receiver { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Phone { get; set; }
        public bool? IsPayment { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
