using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IVoucherRepository _voucherRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _orderRepository = unitOfWork.Order;
            _productRepository = unitOfWork.Product;
            _voucherRepository = unitOfWork.Voucher;
        }

        public async Task<IActionResult> GetOrders(OrderFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _orderRepository.GetAll();
                if (filter.Receiver != null && !filter.Receiver.IsNullOrEmpty())
                {
                    query = query.Where(o => o.Receiver.Equals(filter.Receiver));
                }
                if (filter.Phone != null && !filter.Phone.IsNullOrEmpty())
                {
                    query = query.Where(o => o.Phone.Equals(filter.Phone));
                }
                if (filter.CreateAt != null)
                {
                    query = query.Where(o => o.CreateAt.Equals(filter.CreateAt));
                }
                if (filter.From != null) {
                    query = query.Where(o => o.CreateAt > filter.From);
                }
                if (filter.To != null)
                {
                    query = query.Where(o => o.CreateAt < filter.To);
                }
                if (filter.Status != null && !filter.Status.IsNullOrEmpty())
                {
                    query = query.Where(o => o.Status.Equals(filter.Status));
                }
                if (filter.IsPayment != null)
                {
                    query = query.Where(o => o.IsPayment.Equals(filter.IsPayment));
                }
                if (filter.CustomerId != null) 
                {
                    query = query.Where(o => o.CustomerId.Equals(filter.CustomerId));
                }
                var totalRows = query.Count();
                var result = await query
                    .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();
                return result.ToPaged(pagination, totalRows).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetOrder(Guid id)
        {
            try
            {
                var order = await _orderRepository.Where(x => x.Id == id)
                    .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return order != null ? order.Ok() : AppErrors.RECORD_NOT_FOUND.NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateOrder(Guid customerId, OrderCreateModel model)
        {
            try
            {
                var voucherInvalids = await CheckValidVoucher(model);
                if (voucherInvalids.Count() > 0)
                {
                    return voucherInvalids.BadRequest();
                }
                if (model.PaymentMethod.Equals(PaymentMethods.VNPAY))
                {
                    return AppErrors.INVALID_PAYMENT_METHOD.BadRequest();
                }
                if (model.PaymentMethod.Equals(PaymentMethods.CASH))
                {
                    var order = _mapper.Map<Order>(model);
                    order.CustomerId = customerId;
                    order.IsPayment = false;
                    order.Status = OrderStatuses.PENDING;
                    _orderRepository.Add(order);
                    var result = await _unitOfWork.SaveChangesAsync();
                    if (result > 0)
                    {
                        await CalculateVoucherQuantity(model);
                        return await GetOrder(order.Id);
                    }
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateOrderStatus(OrderStatusUpdateModel model) 
        {
            try
            {
                var order = await _orderRepository
                    .Where(o => o.Id.Equals(model.Id))
                    .FirstOrDefaultAsync();
                if(order == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                _mapper.Map(model, order);
                _orderRepository.Update(order);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetOrder(order.Id);
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        private async Task<ICollection<string>> CheckValidVoucher(OrderCreateModel order)
        {
            try
            {
                var errors = new List<string>();
                foreach (var item in order.OrderVouchers)
                {
                    var voucher = await _voucherRepository.Where(x => x.Id.Equals(item.VoucherId)).FirstOrDefaultAsync();
                    if (voucher != null)
                    {
                        if (voucher.Quantity == 0)
                        {
                            errors.Add($"{AppErrors.VOUCHER_NOT_ENOUGH}: {voucher.Name}");
                        }
                    }
                    else
                    {
                        errors.Add($"{AppErrors.VOUCHER_NOT_EXIST}: {item.VoucherId}");
                    }
                }
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CalculateVoucherQuantity(OrderCreateModel order)
        {
            try
            {
                var vouchers = new List<Voucher>();
                foreach (var item in order.OrderVouchers)
                {
                    var voucher = await _voucherRepository.Where(x => x.Id.Equals(item.VoucherId)).FirstOrDefaultAsync();
                    if (voucher != null)
                    {
                        if (voucher.Quantity > 0)
                        {
                            voucher.Quantity = voucher.Quantity - 1;
                            vouchers.Add(voucher);
                        }
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CalculateProductQuantity(Order order)
        {
            try
            {
                var products = new List<Product>();
                foreach (var item in order.OrderDetails)
                {
                    var product = await _productRepository.Where(x => x.Id.Equals(item.ProductId)).FirstOrDefaultAsync();
                    if (product == null)
                    {
                        continue;
                    }
                    product.Quantity = product.Quantity - item.Quantity;
                    products.Add(product);
                }
                // Update list product da duoc chinh sua tren code
                _productRepository.UpdateRange(products);

                // Luu thay doi xuong database
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
