﻿using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class ProductCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Origin { get; set; } = null!;

        public IFormFile Thumbnail { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public int Price { get; set; }

        public int? PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public ICollection<ProductCategoryCreateModel> ProductCategories { get; set; } = new List<ProductCategoryCreateModel>();
    }
}
