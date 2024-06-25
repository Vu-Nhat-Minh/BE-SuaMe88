using Domain.Entities;

namespace Domain.Models.Views
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Origin { get; set; } = null!;

        public string ThumbnailUrl { get; set; } = null!;

        public string MadeIn { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public int Price { get; set; }

        public int? PromotionPrice { get; set; }

        public DateTime ExpireAt { get; set; }

        public int Quantity { get; set; }

        public int Sold { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<ProductCategoryViewModel> ProductCategories { get; set; } = new List<ProductCategoryViewModel>();
    }
}