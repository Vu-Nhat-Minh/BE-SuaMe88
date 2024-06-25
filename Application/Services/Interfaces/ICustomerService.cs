using Domain.Models.Creates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IActionResult> GetCustomer(Guid id);
        Task<IActionResult> CreateCustomer(CustomerCreateModel model);
    }
}
