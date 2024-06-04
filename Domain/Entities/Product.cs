using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Origin { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string? Ingredient { get; set; }

    public string? SweetLevel { get; set; }

    public string? Flavour { get; set; }

    public string? Sample { get; set; }

    public string? Capacity { get; set; }

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public DateTime ExpireAt { get; set; }

    public Guid StoreId { get; set; }

    public DateTime CreateAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Store Store { get; set; } = null!;
}
