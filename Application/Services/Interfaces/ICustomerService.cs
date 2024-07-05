using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IActionResult> GetCustomer(Guid id);
        Task<IActionResult> CreateCustomer(CustomerCreateModel model);
        Task<IActionResult> GetCustomers(CustomerFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> UpdateCustomer(Guid id, CustomerUpdateModel model);
        Task<IActionResult> ChangePassword(Guid id, PasswordUpdateModel model);
        Task<IActionResult> ChangeStatus(CustomerStatusUpdateModel model);
    }
}
