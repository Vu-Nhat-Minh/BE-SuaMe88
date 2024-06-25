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

namespace Application.Services.Implementations
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _categoryRepository = unitOfWork.Category;
        }

        public async Task<IActionResult> GetCategories(CategoryFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _categoryRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(x => x.Name.Contains(filter.Name));
                }

                var totalRows = query.Count();
                var categories = await query
                    .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();

                return categories.ToPaged(pagination, totalRows).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetCategory(Guid id)
        {
            try
            {
                var category = await _categoryRepository.Where(x => x.Id.Equals(id))
                    .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }

                return category.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateCategory(CategoryCreateModel model)
        {
            try
            {
                var category = _mapper.Map<Category>(model);
                _categoryRepository.Add(category);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetCategory(category.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateCategory(Guid id, CategoryUpdateModel model)
        {
            try
            {
                var category = await _categoryRepository.Where(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }

                _mapper.Map(model, category);
                _categoryRepository.Update(category);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetCategory(id);
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
