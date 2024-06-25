using Domain.Models.Creates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IActionResult> CreateOrder(Guid customerId, OrderCreateModel model);
    }
}
