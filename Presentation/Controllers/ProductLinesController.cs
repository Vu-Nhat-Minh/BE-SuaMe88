using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Creates;
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
        [Route("{id}")]
        public async Task<IActionResult> GetProductLines([FromRoute] Guid id, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _productLineService.GetProductLines(id, pagination);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("get-single/{id}")]
        public async Task<IActionResult> GetProductLine([FromRoute] Guid id)
        {
            try
            {
                return await _productLineService.GetProductLine(id);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("get-all/{id}")]
        public async Task<IActionResult> GetValidProductLines([FromRoute] Guid id, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _productLineService.GetValidProductLines(id, pagination);
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
        [Route("import/{productId}")]
        public async Task<IActionResult> ImportProductLine([FromRoute] Guid productId, [FromBody] ProductLineCreateModel model)
        {
            try
            {
                return await _productLineService.ImportProductLine(productId, model);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }
        //Test xong xóa
        [HttpPut]
        [Route("reduce")]
        public async Task<IActionResult> ReduceProductLineQuantity([FromBody]ICollection<OrderDetailCreateModel> models)
        {
            try
            {
                return await _productLineService.ReduceProductLineQuantity(models);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        
    }
}
