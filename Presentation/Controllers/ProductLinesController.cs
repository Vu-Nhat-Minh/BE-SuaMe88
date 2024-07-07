using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/product-lines")]
    [ApiController]
    public class ProductLinesController : ControllerBase
    {
        private readonly IProductLineService _productLineService;

        public ProductLinesController(IProductLineService productLineService)
        {
            _productLineService = productLineService;
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProductLines([FromRoute] Guid productId, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _productLineService.GetProductLines(productId, pagination);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("get-single/{productLineId}")]
        public async Task<IActionResult> GetProductLine([FromRoute] Guid productLineId)
        {
            try
            {
                return await _productLineService.GetProductLine(productLineId);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("get-valid/{productId}")]
        public async Task<IActionResult> GetValidProductLines([FromRoute] Guid productId, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _productLineService.GetValidProductLines(productId, pagination);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateProductLine([FromRoute] Guid id, [FromBody] ProductLineUpdateModel model)
        {
            try
            {
                return await _productLineService.UpdateProductLine(id, model);
            }
            catch (Exception ex) {
                return ex.Message.InternalServerError();
            } 
        }

        [HttpPost]
        [Route("create/{productId}")]
        public async Task<IActionResult> CreateProductLine([FromRoute] Guid productId, [FromBody] ProductLineCreateModel model)
        {
            try
            {
                return await _productLineService.CreateProductLine(productId, model);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("changes/filter")]
        public async Task<IActionResult> GetChanges([FromBody] ProductLineChangeFilterModel model, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _productLineService.GetChanges(model, pagination);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        //Test xong xóa
        //[HttpPut]
        //[Route("reduce")]
        //public async Task<IActionResult> ReduceProductLineQuantity([FromBody]ICollection<OrderDetailCreateModel> models)
        //{
        //    try
        //    {
        //        return await _productLineService.ReduceProductLineQuantity(models);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.InternalServerError();
        //    }
        //}

        
    }
}
