using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class ProductRevenueViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Origin { get; set; } = null!;

        public string ThumbnailUrl { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public int Sold { get; set; }

        public int Revenue { get; set; }

        public string Status { get; set; } = null!;

        public double Rating { get; set; }

    }
}
