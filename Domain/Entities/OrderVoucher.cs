using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class OrderVoucher
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid VoucherId { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Voucher Voucher { get; set; } = null!;
}
