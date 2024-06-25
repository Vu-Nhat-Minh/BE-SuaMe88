using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class ProductLineRepository : Repository<ProductLine>, IProductLineRepository
    {
        public ProductLineRepository(SuaMe88Context context) : base(context)
        {
        }
    }
}
