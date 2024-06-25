using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ProductLine
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime ImportDate { get; set; }

    public DateTime ExpiredAt { get; set; }

    public int? PromotionPrice { get; set; }

    public virtual Product Product { get; set; } = null!;
}
