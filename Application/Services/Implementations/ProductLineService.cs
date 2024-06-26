using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Helpers;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class ProductLineService : BaseService, IProductLineService
    {
        private readonly IProductLineRepository _productLineRepository;
        private readonly IProductRepository _productRepository;
        public ProductLineService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productLineRepository = unitOfWork.ProductLine;
            _productRepository = unitOfWork.Product;
        }

        public async Task<IActionResult> GetProductLines(Guid productId, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productLineRepository.GetAll();
                var totalRows = query.Count();
                var productLines = await query
                    .Where(p => p.ProductId.Equals(productId))
                    .Paginate(pagination)
                    .ToListAsync();
                return productLines.ToPaged(pagination, totalRows).Ok();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<IActionResult> GetValidProductLines(Guid productId, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productLineRepository.GetAll();
                var totalRows = query.Count();
                var productLines = await query
                    .Where(p => p.ProductId.Equals(productId))
                    .Where(p => p.Quantity > 0)
                    .Where(p => p.ExpiredAt > DateTimeHelper.VnNow)
                    .Paginate(pagination)
                    .ToListAsync();
                return productLines.ToPaged(pagination, totalRows).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetProductLine(Guid id)
        {
            try
            {
                var productLine = await _productLineRepository
                    .Where(x => x.Id.Equals(id))
                    .ProjectTo<ProductLineViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (productLine == null) {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                return productLine.Ok();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<IActionResult> ImportProductLine(Guid productId, ProductLineCreateModel model)
        {
            try
            {
                var check = await _productRepository
                    .Where(p => p.Id.Equals(productId))
                    .FirstOrDefaultAsync();
                if (check == null) 
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                var productLine = _mapper.Map<ProductLine>(model);
                productLine.ProductId = productId;
                _productLineRepository.Add(productLine);
                var result = await _unitOfWork.SaveChangesAsync();
                if(result > 0)
                {
                    return await GetProductLine(productId);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ReduceProductLineQuantity(ProductLineQuantityReductionModel model)
        {
            try
            {
                var productLines = await _productLineRepository
                .Where(pl => !pl.ProductId.Equals(model.productId) && pl.Quantity > 0 && pl.ExpiredAt > DateTimeHelper.VnNow)
                .OrderBy(pl => pl.ExpiredAt)
                .ToListAsync();
                int toReduce = model.quantity;
                foreach (var productLine in productLines)
                {
                    if (toReduce <= 0)
                    {
                        break;
                    }
                    if (productLine.Quantity >= toReduce)
                    {
                        productLine.Quantity -= toReduce;
                        toReduce = 0;
                    }
                    else
                    {
                        toReduce -= productLine.Quantity;
                        productLine.Quantity = 0;
                    }
                }
                if (toReduce > 0)
                {
                    return AppErrors.PRODUCT_INSTOCK_NOT_ENOUGH.UnprocessableEntity();
                }
                await _unitOfWork.SaveChangesAsync();
                return "Trừ hàng kho thành công".Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateProductLine(Guid productId, ProductLineUpdateModel model)
        {
            try
            {
                var productLine = await _productLineRepository
                .Where(p => p.Id.Equals(productId))
                .FirstOrDefaultAsync();
                if (productLine == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                _mapper.Map(model, productLine);
                _productLineRepository.Update(productLine);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetProductLine(productId);
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
    }
}
