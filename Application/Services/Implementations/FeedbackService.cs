﻿using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Application.Services.Implementations
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _feedbackRepository = unitOfWork.Feedback;
            _orderRepository = unitOfWork.Order;
            _orderDetailRepository = unitOfWork.OrderDetail;
        }

        public async Task<IActionResult> GetFeedbacks(FeedbackFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _feedbackRepository.GetAll();
                if (filter.Id != null)
                {
                    query = query.Where(fb => fb.Id.Equals(filter.Id));
                }
                if (filter.ProductID != null)
                {
                    query = query.Where(fb => fb.ProductID.Equals(filter.ProductID));
                }
                if (filter.CustomerId != null)
                {
                    query = query.Where(fb => fb.CustomerId.Equals(filter.CustomerId));
                }
                if (filter.CreateAt != null)
                {
                    query = query.Where(fb => fb.CreateAt.Date.Equals(filter.CreateAt));
                }
                var totalRow = query.Count();
                var feedbacks = await query
                    .ProjectTo<FeedbackViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();
                return feedbacks.ToPaged(pagination, totalRow).Ok();
            }
            catch (Exception)
            {
                throw;            
            }
        }

        public async Task<IActionResult> GetFeedback(Guid id)
        {
            try
            {
                var feedback = await _feedbackRepository
                    .Where(fb => fb.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if(feedback == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                return feedback.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateFeedback(Guid productId, Guid customerId, FeedbackCreateModel model)
        {
            try
            {
                if(!HasCompletedOrder(customerId, productId).Result)
                {
                    return AppErrors.NO_COMPLETED_ORDER.UnprocessableEntity();
                }
                if (HasFeedback(customerId, productId).Result) 
                {
                    return AppErrors.FEEDBACK_ALREADY_EXISTS.UnprocessableEntity();
                }
                var feedback = _mapper.Map<Feedback>(model);
                feedback.ProductID = productId;
                feedback.CustomerId = customerId;
                _feedbackRepository.Add(feedback);
                var result = await _unitOfWork.SaveChangesAsync();
                if(result >0)
                {
                    return await GetFeedback(feedback.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Check if customer has purchased a product
        private async Task<bool> HasCompletedOrder(Guid customerId, Guid productId)
        {
            try
            {
                return await _orderRepository.GetAll()
                    .Join(_orderDetailRepository.GetAll(),
                    order => order.Id,
                    orderDetail => orderDetail.OrderId,
                    (order, orderDetail) => new { order.CustomerId, orderDetail.ProductId })
                    .AnyAsync(x => x.CustomerId == customerId && x.ProductId == productId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Check if a customer has already given feedback to a product
        private async Task<bool> HasFeedback(Guid customerId, Guid productId)
        {
            try 
            {
                return await _feedbackRepository
                    .Where(fb => fb.CustomerId.Equals(customerId) && fb.ProductID.Equals(productId))
                    .AnyAsync();
            }
            catch (Exception) 
            {
                throw;
            }
        }
        
    }
}
