﻿using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Http;
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
        [Route("geti-all/{id}")]
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

        //Test xong xóa
        [HttpPut]
        [Route("reduce")]
        public async Task<IActionResult> ReduceProductLineQuantity([FromBody]ProductLineQuantityReductionModel model)
        {
            try
            {
                return await _productLineService.ReduceProductLineQuantity(model);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        
    }
}