using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class OrderTransaction
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
