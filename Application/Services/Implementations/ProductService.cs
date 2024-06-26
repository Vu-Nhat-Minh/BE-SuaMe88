
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
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICloudStorageService _cloudStorageService;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _productRepository = unitOfWork.Product;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> GetProducts(ProductFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _productRepository.GetAll();
                if (filter.Search != null && !filter.Search.IsNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(filter.Search));
                }
                if (filter.Categories != null && filter.Categories.Count > 0)
                {
                    query = query.Where(x => filter.Categories.All(fc => x.ProductCategories.Select(x => x.CategoryId).Contains(fc)));
                }

                var totalRows = query.Count();
                var categories = await query
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();
                return categories.ToPaged(pagination, totalRows).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.Where(x => x.Id.Equals(id))
                    .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }

                return product.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateProduct(ProductCreateModel model)
        {
            try
            {
                var product = _mapper.Map<Product>(model);
                if (model.Thumbnail != null)
                {
                    product.ThumbnailUrl = await _cloudStorageService.Upload(Guid.NewGuid(), model.Thumbnail);
                }
                _productRepository.Add(product);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetProduct(product.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateProduct(Guid id, ProductUpdateModel model)
        {
            try
            {
                var product = await _productRepository.Where(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }

                _mapper.Map(model, product);
                if (model.Thumbnail != null)
                {
                    product.ThumbnailUrl = await _cloudStorageService.Upload(Guid.NewGuid(), model.Thumbnail);
                }
                _productRepository.Update(product);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetProduct(id);
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
