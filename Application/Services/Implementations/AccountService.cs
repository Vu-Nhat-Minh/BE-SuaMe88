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
    public class AccountService : BaseService, IAccountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IAdminRepository _adminRepository;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _customerRepository = unitOfWork.Customer;
            _staffRepository = unitOfWork.Staff;
            _adminRepository = unitOfWork.Admin;
        }

        //Customer
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
                _mapper.Map(model, customer);
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
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

        public async Task<IActionResult> ChangeCustomerPassword(Guid id, PasswordUpdateModel model)
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

        public async Task<IActionResult> ChangeCustomerStatus(CustomerStatusUpdateModel model)
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
                if (result > 0)
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

        //Staff
        public async Task<IActionResult> GetStaffs(StaffFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _staffRepository.GetAll();
                if (filter.Username != null && !filter.Username.IsNullOrEmpty())
                {
                    query = query.Where(s => s.Username.Contains(filter.Username));
                }
                if (filter.Name != null && !filter.Name.IsNullOrEmpty())
                {
                    query = query.Where(s => s.Name.Contains(filter.Name));
                }
                if(filter.Status != null && !filter.Status.IsNullOrEmpty())
                {
                    query= query.Where(s => s.Status.Equals(filter.Status));
                }
                var totalRow = query.Count();
                var results = await query
                    .Paginate(pagination)
                    .ProjectTo<StaffViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return results.ToPaged(pagination, totalRow).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStaff(Guid id)
        {
            try
            {
                var staff = await _staffRepository
                    .Where(s => s.Id.Equals(id))
                    .ProjectTo<StaffViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if(staff == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                return staff.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateStaff(Guid id, StaffUpdateModel model)
        {
            try
            {
                var staff = await _staffRepository
                    .Where(s => s.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if (staff == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                _mapper.Map(model, staff);
                _staffRepository.Update(staff);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetStaff(id);
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ChangeStaffStatus(StaffStatusUpdateModel model)
        {
            try
            {
                var staff = await _staffRepository
                    .Where(s => s.Id.Equals(model.StaffId))
                    .FirstOrDefaultAsync();
                if (staff == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                if (model.Status != null)
                {
                    if (model.Status.Equals(CustomerStatuses.ACTIVE))
                    {
                        if (staff.Status.Equals(CustomerStatuses.ACTIVE))
                        {
                            return AppErrors.NO_CHANGE.UnprocessableEntity();
                        }
                        staff.Status = CustomerStatuses.INACTIVE;
                    }
                    else if (model.Status.Equals(CustomerStatuses.INACTIVE))
                    {
                        if (staff.Status.Equals(CustomerStatuses.INACTIVE))
                        {
                            return AppErrors.NO_CHANGE.UnprocessableEntity();
                        }
                        staff.Status = CustomerStatuses.INACTIVE;
                    }
                    else
                    {
                        return AppErrors.INVALID_STATUS.UnprocessableEntity();
                    }
                }
                _staffRepository.Update(staff);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
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

        public async Task<IActionResult> CreateStaff(StaffCreateModel model)
        {
            try
            {
                if (IsStaffExisting(model.Username))
                {
                    return AppErrors.USERNAME_EXIST.Conflict();
                }
                var staff = _mapper.Map<Staff>(model);
                _staffRepository.Add(staff);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetStaff(staff.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> ChangeStaffPassword(Guid id, PasswordUpdateModel model)
        {
            try
            {
                var staff = await _staffRepository
                    .Where(c => c.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if (staff == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                if (!model.OldPassword.Equals(staff.Password))
                {
                    return AppErrors.WRONG_PASSWORD.UnprocessableEntity();
                }
                if (model.OldPassword.Equals(model.NewPassword))
                {
                    return AppErrors.SAME_PASSOWRD.UnprocessableEntity();
                }

                staff.Password = model.NewPassword;
                _staffRepository.Update(staff);
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

        public async Task<IActionResult> GetAdmins(AdminFilterModel model, PaginationRequestModel pagination)
        {
            try
            {

                var query = _adminRepository.GetAll();
                if (model.Username != null && !model.Username.IsNullOrEmpty())
                {
                    query = query.Where(a => a.Username.Contains(model.Username));
                }
                var totalRow = query.Count();
                var results = await query
                    .ProjectTo<AdminViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();
                return results.ToPaged(pagination, totalRow).Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetAdmin(Guid id)
        {
            try
            {
                var admin = await _adminRepository
                    .Where(a => a.Id.Equals(id))
                    .ProjectTo<AdminViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (admin == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                return admin.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateAdmin(AdminCreateModel model)
        {
            try
            {
                if (IsAdminExisting(model.Username))
                {
                    return AppErrors.USERNAME_EXIST.UnprocessableEntity();
                }
                var admin = _mapper.Map<Admin>(model);
                _adminRepository.Add(admin);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetAdmin(admin.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsAdminExisting(string username)
        {
            try
            {
                return _adminRepository.Any(x => x.Username.Equals(username));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsCustomerExists(string username)
        {
            try
            {
                return _customerRepository.Any(x => x.Username.Equals(username));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsStaffExisting(string username)
        {
            try
            {
                return _staffRepository.Any(x => x.Username.Equals(username));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
