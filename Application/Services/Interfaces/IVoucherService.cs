using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<IActionResult> GetVouchers(VoucherFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetVoucher(Guid id);
        Task<IActionResult> CreateVoucher(VoucherCreateModel model);
        Task<IActionResult> UpdateVoucher(Guid id, VoucherUpdateModel model);
    }
}
