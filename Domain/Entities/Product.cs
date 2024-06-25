using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Origin { get; set; } = null!;

    public string ThumbnailUrl { get; set; } = null!;

    public string MadeIn { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public int AgeFrom { get; set; }

    public int AgeTo { get; set; }

    public int Price { get; set; }

    public int? PromotionPrice { get; set; }

    public DateTime ExpireAt { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
