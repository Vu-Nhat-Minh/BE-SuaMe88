using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class ProductLineViewModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTime ImportDate { get; set; }

        public DateTime ExpiredAt { get; set; }

        public int? PromotionPrice { get; set; }
    }
}
