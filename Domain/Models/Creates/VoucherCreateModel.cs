using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Creates
{
    public class VoucherCreateModel
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ThumbnailUrl { get; set; } = null!;

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int? MinOrderValue { get; set; }

        public int Value { get; set; }

        public int Quantity { get; set; }
    }
}
