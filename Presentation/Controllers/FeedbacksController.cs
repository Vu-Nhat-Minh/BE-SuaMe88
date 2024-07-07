using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetFeedbacks([FromBody]FeedbackFilterModel model, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _feedbackService.GetFeedbacks(model, pagination);
            }
            catch (Exception ex) 
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetFeedback(Guid id)
        {
            try
            {
                return await _feedbackService.GetFeedback(id);
            }
            catch (Exception ex) 
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("create/")]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateModel model)
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _feedbackService.CreateFeedback(auth.Id, model);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

        [HttpGet]
        [Route("check/{productId}")]
        [Authorize]
        public async Task<IActionResult> HasFeedback([FromRoute] Guid productId)
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                bool result = await _feedbackService.HasFeedback(auth.Id, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex.Message.InternalServerError();
            }
        }

    }
}
