using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid CustomerId { get; set; }

    public string? Message { get; set; }

    public int Star { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
