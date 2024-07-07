using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class StatisticsService : BaseService, IStatisticService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public StatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _transactionRepository = unitOfWork.Transaction;
            _orderRepository = unitOfWork.Order;
            _productRepository = unitOfWork.Product;
        }

        public async Task<IActionResult> GetProductRevenues(ProductRevenueFilterModel model, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productRepository.GetAll();
                if(model.Id != null)
                {
                    query = query.Where(p => p.Id.Equals(model.Id));
                }
                if(model.Search != null && !model.Search.IsNullOrEmpty())
                {
                    query = query.Where(p => p.Name.Contains(model.Search) || p.Origin.Contains(model.Search) || p.Brand.Contains(model.Search));
                }
                if (model.Status != null && model.Status.IsNullOrEmpty())
                {
                    query = query.Where(p => p.Status.Equals(model.Status));
                }
                var result = await query
                    .ProjectTo<ProductRevenueViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return result.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
