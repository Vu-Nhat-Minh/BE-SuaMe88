using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class ProductLineChangeViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid ProductLineId { get; set; }

        public int Quantity { get; set; }

        public bool IsImport { get; set; }

        public string? Purpose { get; set; }
    }
}
