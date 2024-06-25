using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class OrderDetailViewModel
    {
        public Guid Id { get; set; }

        public ProductViewModel Product { get; set; } = null!;

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
