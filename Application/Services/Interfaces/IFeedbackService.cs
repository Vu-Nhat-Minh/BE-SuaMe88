using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<IActionResult> GetFeedbacks(FeedbackFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetFeedback(Guid id);
        Task<IActionResult> CreateFeedback(Guid orderId, Guid customerId, FeedbackCreateModel model);

    }
}
