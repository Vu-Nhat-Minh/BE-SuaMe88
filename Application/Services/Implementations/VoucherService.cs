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
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class VoucherService : BaseService, IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _voucherRepository = unitOfWork.Voucher;
        }

        public async Task<IActionResult> GetVouchers(VoucherFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _voucherRepository.GetAll();
            if(filter.Status != null && !filter.Status.IsNullOrEmpty())
            {
                query = query.Where(v => v.Status == filter.Status);
            }
            if (filter.Search != null && !filter.Search.IsNullOrEmpty()) { 
                query = query.Where(v => v.Code.Equals(filter.Search));
            }
            if (filter.MinOrderValue != null)
            {
                query = query.Where(v => v.MinOrderValue >= filter.MinOrderValue);
            }
            if (filter.From != null) 
            { 
                query = query.Where(v => v.From >= filter.From);
            }
            if (filter.To != null)
            {
                query = query.Where(v => v.To <= filter.To);
            }
            var totalRows = query.Count();
            var result = await query
                .ProjectTo<VoucherViewModel>(_mapper.ConfigurationProvider)
                .Paginate(pagination)
                .ToListAsync();
            if(result.Count > 0)
            {
                return result.ToPaged(pagination, totalRows).Ok();
            }
            return AppErrors.VOUCHER_NOT_EXIST.NotFound();
        }

        public async Task<IActionResult> GetVoucher(Guid id)
        {
            try
            {
                var voucher = await _voucherRepository
                .Where(v => v.Id == id)
                .ProjectTo<VoucherViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                if (voucher != null) 
                {
                    return voucher.Ok();
                }
                return AppErrors.VOUCHER_NOT_EXIST.NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateVoucher(VoucherCreateModel model)
        {
            try
            {
                var voucher = _mapper.Map<Voucher>(model);
                _voucherRepository.Add(voucher);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetVoucher(voucher.Id);
                }
                return AppErrors.CREATE_FAIL.UnprocessableEntity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateVoucher(Guid id, VoucherUpdateModel model)
        {
            try
            {
                var voucher = await _voucherRepository
                .Where(v => v.Id.Equals(id))
                .FirstOrDefaultAsync();
                if (voucher == null)
                {
                    return AppErrors.RECORD_NOT_FOUND.NotFound();
                }
                _mapper.Map(model, voucher);
                _voucherRepository.Update(voucher);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetVoucher(id);
                }
                return AppErrors.UPDATE_FAIL.UnprocessableEntity();
            } 
            catch (Exception)
            {
                throw;
            }
        }
    }
}
