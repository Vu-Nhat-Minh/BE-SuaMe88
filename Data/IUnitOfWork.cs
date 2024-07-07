
using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductCategoryRepository ProductCategory { get; }
        public IOrderRepository Order { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IFeedbackRepository Feedback { get; }
        public IAdminRepository Admin { get; }
        public IStaffRepository Staff { get; }
        public ICustomerRepository Customer { get; }
        public IProductRepository Product { get; }
        public IVoucherRepository Voucher { get; }
        public IProductLineRepository ProductLine { get; }
        public IProductLineChangeRepository ProductLineChange { get; }
        public ITransactionRepository Transaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Dispose();
        Task<int> SaveChangesAsync();
    }
}
