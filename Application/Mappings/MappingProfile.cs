using AutoMapper;
using Common.Helpers;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Authentications;
using Domain.Models.Creates;
using Domain.Models.Updates;
using Domain.Models.Views;
using Google.Apis.Storage.v1;
using Microsoft.EntityFrameworkCore.Update.Internal;

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
            CreateMap<Admin, AuthModel>();
            CreateMap<Staff, AuthModel>();

            // Admin
            CreateMap<Admin, AdminViewModel>();

            // Staff
            CreateMap<Staff, StaffViewModel>();
            CreateMap<StaffCreateModel, Staff>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatuses.ACTIVE));
            CreateMap<StaffUpdateModel, Staff>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // Customer
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerCreateModel, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatuses.ACTIVE))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));
            CreateMap<CustomerUpdateModel, Customer>();
            // Category
            CreateMap<Category, CategoryViewModel>();

            // Product
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.ProductLines.Where(x => x.ExpiredAt > DateTimeHelper.VnNow).Sum(x => x.Quantity)))
                .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Feedbacks.Any() ? Math.Round(src.Feedbacks.Average(f => (double)f.Star), 1) : 0))
                //.ForMember(dest => dest.Sold, opt => opt.MapFrom(src => src.OrderDetails.Sum(x => x.Quantity)));
                .ForMember(dest => dest.Sold, opt => opt.MapFrom(src =>
                src.OrderDetails.Where(od => od.Order.Status.Equals(OrderStatuses.COMPLETED)).Sum(od => od.Quantity)));

            CreateMap<ProductCreateModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProductStatuses.ACTIVE))
                //.ForMember(dest => dest.MadeIn, opt => opt.MapFrom(src => src.Origin))
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom((src, dest) => src.ProductCategories.Select(x =>
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = dest.Id,
                    CategoryId = x.CategoryId,
                }).ToList()));
            CreateMap<ProductUpdateModel, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Product, ProductRevenueViewModel>()
                .ForMember(dest => dest.Sold, opt => opt.MapFrom(src =>
                src.OrderDetails.Where(od => od.Order.Status.Equals(OrderStatuses.COMPLETED)).Sum(od => od.Quantity)))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src =>
                src.OrderDetails.Where(od => od.Order.Status.Equals(OrderStatuses.COMPLETED)).Sum(od => od.Price)));

            // Product Category
            CreateMap<ProductCategory, ProductCategoryViewModel>();

            //ProductLine
            CreateMap<ProductLineCreateModel, ProductLine>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ImportDate, opt => opt.MapFrom(src => DateTimeHelper.VnNow));
            CreateMap<ProductLine, ProductLineViewModel>()
                .ForMember(dest => dest.ImportDate, opt => opt.MapFrom(src => src.ImportDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.ExpiredAt, opt => opt.MapFrom(src => src.ExpiredAt.ToString("dd/MM/yyyy")));
            CreateMap<ProductLineUpdateModel, ProductLine>();

            //ProductLineChange
            CreateMap<ProductLineChange, ProductLineChangeViewModel>()
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt.ToString("dd/MM/yyyy")));
            CreateMap<ProductLineChangeCreateModel, ProductLineChange>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));

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

            //Voucher
            CreateMap<VoucherCreateModel, Voucher>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => VoucherStatuses.ACTIVE))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));
            CreateMap<Voucher, VoucherViewModel>();
            CreateMap<VoucherUpdateModel, Voucher>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Feedback
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<FeedbackCreateModel, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTimeHelper.VnNow));
        }
    }
}
