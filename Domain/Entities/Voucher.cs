using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Voucher
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ThumbnailUrl { get; set; } = null!;

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public int? MinOrderValue { get; set; }

    public int Value { get; set; }

    public int Quantity { get; set; }

    public DateTime CreateAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderVoucher> OrderVouchers { get; set; } = new List<OrderVoucher>();
}
