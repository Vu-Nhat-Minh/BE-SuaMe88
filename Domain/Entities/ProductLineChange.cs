using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ProductLineChange
{
    public Guid Id { get; set; }

    public DateTime CreateAt { get; set; }

    public Guid ProductLineId { get; set; }

    public int Quantity { get; set; }

    public bool IsImport { get; set; }

    public string? Purpose { get; set; }

    public virtual ProductLine ProductLine { get; set; } = null!;
}
