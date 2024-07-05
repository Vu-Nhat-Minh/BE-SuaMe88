using Application.Services.Interfaces;
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
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _customerRepository = unitOfWork.Customer;
        }

        public async Task<IActionResult> GetCustomer(Guid id)
        {
            try
            {
                var customer = await _customerRepository.Where(x => x.Id.Equals(id))
                    .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return customer != null ? customer.Ok() : AppErrors.RECORD_NOT_FOUND.NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetCustomers(CustomerFilterModel model, PaginationRequestModel pagination) 
        {
            try
            {
                var query = _customerRepository.GetAll();
                if (model.Username != null && !model.Username.IsNullOrEmpty())
                {
                    query = query.Where(c => c.Username.Contains(model.Username));
                }
                if (model.Name != null && !model.Name.IsNullOrEmpty())
                {
                    query = query.Where(c => c.Name.Contains(model.Name));
                }
                if (model.Phone != null && !model.Phone.IsNullOrEmpty())
                {
                    query = query.Where(c => c.Phone.Equals(model.Phone));
                }
                if (model.Status != null && !model.Status.IsNullOrEmpty())
                {
                    query = query.Where(c => c.Status.Equals(model.Status));
                }
                var totalRow = query.Count();
                var result = await query
                    .Paginate(pagination)
                    .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return result.ToPaged(pagination, totalRow).Ok();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateCustomer(Guid id, CustomerUpdateModel model) 
        {
            try
            {
                var customer = await _customerRepository
                    .Where(c => c.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if (customer == null) 
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                _mapper.Map<Customer>(model);
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChangesAsync();
                if(result > 0)
                {
                    return await GetCustomer(id);
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<IActionResult> ChangePassword(Guid id, PasswordUpdateModel model)
        {
            try
            {
                var customer = await _customerRepository
                    .Where(c => c.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if (customer == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                if (!model.OldPassword.Equals(customer.Password))
                {
                    return AppErrors.WRONG_PASSWORD.UnprocessableEntity();
                }
                if (model.OldPassword.Equals(model.NewPassword))
                {
                    return AppErrors.SAME_PASSOWRD.UnprocessableEntity();
                }

                customer.Password = model.NewPassword;
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return AppNotifications.UPDATED_PASSWORD.Ok();
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ChangeStatus (CustomerStatusUpdateModel model)
        {
            try
            {
                var customer = await _customerRepository
                    .Where(c => c.Id.Equals(model.CustomerId))
                    .FirstOrDefaultAsync();
                if (customer == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                if (model.Status != null)
                {
                    if (model.Status.Equals(CustomerStatuses.ACTIVE))
                    {
                        if (customer.Status.Equals(CustomerStatuses.ACTIVE))
                        {
                            return AppErrors.NO_CHANGE.UnprocessableEntity();
                        }
                        customer.Status = CustomerStatuses.INACTIVE;
                    }
                    else if (model.Status.Equals(CustomerStatuses.INACTIVE))
                    {
                        if (customer.Status.Equals(CustomerStatuses.INACTIVE))
                        {
                            return AppErrors.NO_CHANGE.UnprocessableEntity();
                        }
                        customer.Status = CustomerStatuses.INACTIVE;
                    }
                    else
                    {
                        return AppErrors.INVALID_STATUS.UnprocessableEntity();
                    }
                }
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChangesAsync();
                if(result > 0)
                {
                    return AppNotifications.UPDATED_STATUS.Ok();
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateCustomer(CustomerCreateModel model)
        {
            try
            {
                if (IsCustomerExists(model.Username))
                {
                    return AppErrors.USERNAME_EXIST.Conflict();
                }
                var customer = _mapper.Map<Customer>(model);
                _customerRepository.Add(customer);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetCustomer(customer.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsCustomerExists(string username) { 
            try
            {
                return _customerRepository.Any(x => x.Username.Equals(username));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
