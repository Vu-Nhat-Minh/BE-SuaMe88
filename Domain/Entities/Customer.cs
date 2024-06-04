using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Customer
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string Name { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string Rank { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderTransaction> OrderTransactions { get; set; } = new List<OrderTransaction>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
