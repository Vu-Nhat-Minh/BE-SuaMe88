using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string TargetAudience { get; set; } = null!;

    public string AgeRange { get; set; } = null!;

    public string MilkType { get; set; } = null!;

    public string? Icon { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
