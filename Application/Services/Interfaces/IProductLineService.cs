using Domain.Models.Creates;
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
         Task<IActionResult> ImportProductLine(Guid productId, ProductLineCreateModel model);        
         Task<IActionResult> ReduceProductLineQuantity(ProductLineQuantityReductionModel model);        
         Task<IActionResult> UpdateProductLine(Guid productId, ProductLineUpdateModel model);
    }
}
