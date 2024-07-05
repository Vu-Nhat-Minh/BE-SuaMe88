using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IActionResult> GetCustomer(Guid id);
        Task<IActionResult> CreateCustomer(CustomerCreateModel model);
        Task<IActionResult> GetCustomers(CustomerFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> UpdateCustomer(Guid id, CustomerUpdateModel model);
        Task<IActionResult> ChangeCustomerPassword(Guid id, PasswordUpdateModel model);
        Task<IActionResult> ChangeCustomerStatus(CustomerStatusUpdateModel model);
        Task<IActionResult> GetStaffs(StaffFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetStaff(Guid id);
        Task<IActionResult> UpdateStaff(Guid id, StaffUpdateModel model);
        Task<IActionResult> ChangeStaffStatus(StaffStatusUpdateModel model);
        Task<IActionResult> CreateStaff(StaffCreateModel model);
        Task<IActionResult> ChangeStaffPassword(Guid id, PasswordUpdateModel model);
    }
}
