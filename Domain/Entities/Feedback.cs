using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public string Content { get; set; } = null!;

    public int RateStar { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
