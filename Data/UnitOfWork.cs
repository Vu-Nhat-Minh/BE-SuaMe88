using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SuaMe88Context _context;
        private IDbContextTransaction _transaction = null!;

        public UnitOfWork(SuaMe88Context context)
        {
            _context = context;
        }

        public ICategoryRepository _category = null!;

        public ICategoryRepository Category
        {
            get { return _category ??= new CategoryRepository(_context); }
        }

        public IProductCategoryRepository _productCateogry = null!;

        public IProductCategoryRepository ProductCategory
        {
            get { return _productCateogry ??= new ProductCategoryRepository(_context); }
        }

        public IOrderRepository _order = null!;

        public IOrderRepository Order
        {
            get { return _order ??= new OrderRepository(_context); }
        }

        public IOrderDetailRepository _orderDetail = null!;

        public IOrderDetailRepository OrderDetail
        {
            get { return _orderDetail ??= new OrderDetailRepository(_context); }
        }

        public IFeedbackRepository _feedback = null!;

        public IFeedbackRepository Feedback
        {
            get { return _feedback ??= new FeedbackRepository(_context); }
        }

        public IAdminRepository _admin = null!;

        public IAdminRepository Admin
        {
            get { return _admin ??= new AdminRepository(_context); }
        }

        public ICustomerRepository _customer = null!;

        public ICustomerRepository Customer
        {
            get { return _customer ??= new CustomerRepository(_context); }
        }

        public IProductRepository _product = null!;

        public IProductRepository Product
        {
            get { return _product ??= new ProductRepository(_context); }
        }

        public IVoucherRepository _voucher = null!;

        public IVoucherRepository Voucher
        {
            get { return _voucher ??= new VoucherRepository(_context); }
        }

        public IProductLineRepository _productLine = null!;

        public IProductLineRepository ProductLine
        {
            get { return _productLine ??= new ProductLineRepository(_context); }
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null!;
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null!;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
