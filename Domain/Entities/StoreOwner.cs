using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StoreOwner
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string Name { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public Guid? StoreId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<MembershipTransaction> MembershipTransactions { get; set; } = new List<MembershipTransaction>();

    public virtual Store? Store { get; set; }

    public virtual ICollection<StoreOwnerMembership> StoreOwnerMemberships { get; set; } = new List<StoreOwnerMembership>();
}
