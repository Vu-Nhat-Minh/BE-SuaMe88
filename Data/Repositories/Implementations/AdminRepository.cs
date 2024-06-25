using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(SuaMe88Context context) : base(context)
        {
        }
    }
}
