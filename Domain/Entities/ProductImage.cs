using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ProductImage
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public Guid ProductId { get; set; }

    public bool IsPrimary { get; set; }

    public virtual Product Product { get; set; } = null!;
}
