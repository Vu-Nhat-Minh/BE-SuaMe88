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
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations
{
    public class ProductLineService : BaseService, IProductLineService
    {
        private readonly IProductLineRepository _productLineRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductLineChangeRepository _productLineChangeRepository;
        public ProductLineService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productLineRepository = unitOfWork.ProductLine;
            _productRepository = unitOfWork.Product;
            _productLineChangeRepository = unitOfWork.ProductLineChange;
        }

        public async Task<IActionResult> GetProductLines(Guid productId, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productLineRepository.GetAll();
                var totalRows = query.Count();
                var productLines = await query
                    .Where(p => p.ProductId.Equals(productId))
                    .ProjectTo<ProductLineViewModel>(_mapper.ConfigurationProvider)
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

        public async Task<IActionResult> CreateProductLine(Guid productId, ProductLineCreateModel model)
        {
            try
            {
                var product = await _productRepository
                    .Where(p => p.Id.Equals(productId))
                    .FirstOrDefaultAsync();
                if (product == null) 
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                var productLine = _mapper.Map<ProductLine>(model);
                productLine.ProductId = productId;
                _productLineRepository.Add(productLine);
                var result = await _unitOfWork.SaveChangesAsync();
                if(result > 0)
                {
                    var change = new ProductLineChange
                    {
                        Id = Guid.NewGuid(),
                        CreateAt = DateTimeHelper.VnNow,
                        ProductLineId = productLine.Id,
                        IsImport = true,
                        Quantity = productLine.Quantity,
                        Purpose = "import",
                    };
                    _productLineChangeRepository.Add(change);
                    var changed = await _unitOfWork.SaveChangesAsync();
                    if (changed > 0) 
                    {
                        return await GetProductLine(productLine.Id);
                    }                    
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Tracking
        public async Task<IActionResult> GetChanges(ProductLineChangeFilterModel model, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productLineChangeRepository.GetAll();
                if (model.From != null)
                {
                    query = query.Where(q => q.CreateAt >= model.From);
                }
                if (model.To != null)
                {
                    query = query.Where(q => q.CreateAt <= model.From);
                }
                if (model.ProductLineId != null)
                {
                    query = query.Where(q => q.ProductLineId.Equals(model.ProductLineId));
                }
                if (model.Purpose != null && !model.Purpose.IsNullOrEmpty())
                {
                    query = query.Where(q => q.Purpose.Contains(model.Purpose));
                }
                var results = await query
                    .Paginate(pagination)
                    .ProjectTo<ProductLineChangeViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return results.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ReduceProductLineQuantity(ICollection<OrderDetail> models, string purpose)
        {
            try
            {
                foreach (var model in models)
                {
                    var productLineTarget = new ProductLineQuantityChangeModel
                    {
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                    };

                    // Fetch matching product lines
                    var productLines = await _productLineRepository
                        .Where(pl => pl.ProductId.Equals(productLineTarget.ProductId) && pl.Quantity > 0 && pl.ExpiredAt > DateTimeHelper.VnNow)
                        .OrderBy(pl => pl.ExpiredAt)
                        .ToListAsync();

                    int toReduce = productLineTarget.Quantity;

                    int availableInventory = productLines.Sum(pl => pl.Quantity);
                    if (toReduce > availableInventory) {
                        return AppErrors.PRODUCT_INSTOCK_NOT_ENOUGH.UnprocessableEntity();
                    }

                    foreach (var productLine in productLines)
                    {
                        if (toReduce <= 0)
                        {
                            break;
                        }

                        if (productLine.Quantity >= toReduce)
                        {
                            productLine.Quantity -= toReduce;

                            //Record changes
                            var productLineChange = new ProductLineChange
                            {
                                Id = Guid.NewGuid(),
                                ProductLineId = productLine.Id,
                                Quantity = toReduce, // the reduced quantity
                                IsImport = false,
                                Purpose = purpose,
                                CreateAt = DateTimeHelper.VnNow,
                            };
                            _productLineChangeRepository.Add(productLineChange);

                            toReduce = 0;
                        }
                        else
                        {
                            toReduce -= productLine.Quantity;

                            //Record changes 
                            var productLineChange = new ProductLineChange
                            {
                                Id = Guid.NewGuid(),
                                ProductLineId = productLine.Id,
                                Quantity = productLine.Quantity, // the reduced quantity
                                IsImport = false,
                                Purpose = purpose,
                                CreateAt = DateTimeHelper.VnNow,
                            };
                            _productLineChangeRepository.Add(productLineChange);

                            productLine.Quantity = 0;
                        }
                        
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                return "Trừ hàng kho thành công".Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ReturnProductLineQuantity(Guid id)
        {
            try
            {
                //Fetch adjusted product lines
                var changes = await _productLineChangeRepository
                    .Where(c => c.Purpose.Equals("purchase: " + id.ToString().ToLower()))
                    .ToListAsync();
                foreach(var change in changes)
                {
                    var productLine = await _productLineRepository
                        .Where(pl => pl.Id.Equals(change.ProductLineId))
                        .FirstOrDefaultAsync();
                    if (productLine == null) {
                        return AppErrors.RECORD_NOT_FOUND.NotFound();
                    }
                    productLine.Quantity += change.Quantity;
                    var productLineChange = new ProductLineChange
                    {
                        Id = Guid.NewGuid(),
                        ProductLineId = productLine.Id,
                        Quantity = change.Quantity, // the returned quantity
                        IsImport = true,
                        Purpose = "return: " + id.ToString(),
                        CreateAt = DateTimeHelper.VnNow,
                    };
                    _productLineChangeRepository.Add(productLineChange);
                    _productLineRepository.Update(productLine);
                }        
                
                await _unitOfWork.SaveChangesAsync();
                return "Trả hàng kho thành công".Ok();
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
