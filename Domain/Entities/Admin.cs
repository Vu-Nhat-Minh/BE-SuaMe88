using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Admin
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? AvatarUrl { get; set; }
}
