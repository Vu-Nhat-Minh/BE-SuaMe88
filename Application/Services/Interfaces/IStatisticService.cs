using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<IActionResult> GetProductRevenues(ProductRevenueFilterModel model, PaginationRequestModel pagination);
    }
}
