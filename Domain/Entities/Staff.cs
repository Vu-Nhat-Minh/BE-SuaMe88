using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Staff
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }
}
