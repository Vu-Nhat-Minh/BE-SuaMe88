using Data.Repositories.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ProductLineChangeRepository : Repository<ProductLineChange>, IProductLineChangeRepository
    {
        public ProductLineChangeRepository(SuaMe88Context context) : base(context)
        {
        }
    }
}
