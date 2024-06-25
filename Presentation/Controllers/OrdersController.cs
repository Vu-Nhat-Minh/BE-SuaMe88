using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Creates;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService  orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateModel model)
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _orderService.CreateOrder(auth.Id, model);
            }
            catch (Exception ex) {
                return ex.Message.InternalServerError();
            }
        }
    }
}
