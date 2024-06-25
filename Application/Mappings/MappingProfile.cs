using AutoMapper;
using Common.Helpers;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Authentications;
using Domain.Models.Creates;
using Domain.Models.Updates;
using Domain.Models.Views;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Data type
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);

            // Auth
            CreateMap<Customer, AuthModel>();

            // Customer
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerCreateModel, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatuses.ACTIVE))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));

            // Category
            CreateMap<Category, CategoryViewModel>();

            // Product
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Sold, opt => opt.MapFrom(src => src.OrderDetails.Sum(x => x.Quantity)));
            CreateMap<ProductCreateModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProductStatuses.ACTIVE))
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom((src, dest) => src.ProductCategories.Select(x =>
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = dest.Id,
                    CategoryId = x.CategoryId,
                })));
            CreateMap<ProductUpdateModel, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Order
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderCreateModel, Order>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom((src, dest) => src.OrderDetails.Select(x =>
                new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    OrderId = dest.Id,
                })))
                .ForMember(dest => dest.OrderVouchers, opt => opt.MapFrom((src, dest) => src.OrderVouchers.Select(x =>
                new OrderVoucher
                {
                    Id = Guid.NewGuid(),
                    OrderId = dest.Id,
                    VoucherId = x.VoucherId,
                    CreateAt = DateTimeHelper.VnNow,
                })));
            CreateMap<OrderStatusUpdateModel, Order>();

            // OrderDetail
            CreateMap<OrderDetail, OrderDetailViewModel>();

            // Product Category
            CreateMap<ProductCategory, ProductCategoryViewModel>();

            //Voucher
            CreateMap<VoucherCreateModel, Voucher>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => VoucherStatuses.ACTIVE))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));
            CreateMap<Voucher, VoucherViewModel>();
            CreateMap<VoucherUpdateModel, Voucher>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
