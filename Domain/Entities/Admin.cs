using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Admin
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
