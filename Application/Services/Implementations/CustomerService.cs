using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
