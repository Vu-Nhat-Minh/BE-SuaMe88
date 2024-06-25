using Application.Services.Interfaces;
using AutoMapper;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Creates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Implementations
{
    public class ProductLineService : BaseService, IProductLineService
    {
        private readonly IProductLineRepository _repository;
        public ProductLineService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _repository = unitOfWork.ProductLine;
        }

        //public async Task<IActionResult> ImportProductLine(Guid productId, ProductLineCreateModel model)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception) 
        //    {
        //        throw;
        //    }
        //}
    }
}
