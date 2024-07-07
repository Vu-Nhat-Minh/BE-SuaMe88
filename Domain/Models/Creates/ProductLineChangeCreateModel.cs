using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Creates
{
    public class ProductLineChangeCreateModel
    {
        public Guid ProductLineId { get; set; }
        public int Quantity { get; set; }
        public string Purpose { get; set; }
        public string IsImport { get; set; }
    }
}
