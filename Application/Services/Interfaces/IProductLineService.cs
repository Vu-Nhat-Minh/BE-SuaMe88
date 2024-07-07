using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IProductLineService
    {
        Task<IActionResult> GetProductLines(Guid productId, PaginationRequestModel pagination);
        Task<IActionResult> GetValidProductLines(Guid productId, PaginationRequestModel pagination);
        Task<IActionResult> GetProductLine(Guid id);        
        Task<IActionResult> CreateProductLine(Guid productId, ProductLineCreateModel model);
        Task<IActionResult> ReduceProductLineQuantity(ICollection<OrderDetail> models, string purpose);
        Task<IActionResult> ReturnProductLineQuantity(Guid id);
        Task<IActionResult> UpdateProductLine(Guid productId, ProductLineUpdateModel model);
        Task<IActionResult> GetChanges(ProductLineChangeFilterModel model, PaginationRequestModel pagination);
    }
}
