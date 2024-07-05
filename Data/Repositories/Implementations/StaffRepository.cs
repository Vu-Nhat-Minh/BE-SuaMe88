using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(SuaMe88Context context) : base(context)
        {
        }
    }
}
