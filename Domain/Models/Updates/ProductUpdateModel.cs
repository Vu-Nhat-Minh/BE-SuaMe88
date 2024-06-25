using Microsoft.AspNetCore.Http;

namespace Domain.Models.Updates
{
    public class ProductUpdateModel
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Origin { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public string? MadeIn { get; set; }

        public string? Brand { get; set; }

        public int? Price { get; set; }

        public int? PromotionPrice { get; set; }

        public DateTime? ExpireAt { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }
    }
}