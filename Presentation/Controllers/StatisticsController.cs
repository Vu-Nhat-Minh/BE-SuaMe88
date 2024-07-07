using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpPost]
        [Route("revenues")]
        public async Task<IActionResult> GetProductrevenues([FromBody]ProductRevenueFilterModel model,[FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _statisticService.GetProductRevenues(model, pagination);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }
    }
}
