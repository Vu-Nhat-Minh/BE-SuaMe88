using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Membership
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public virtual ICollection<MembershipTransaction> MembershipTransactions { get; set; } = new List<MembershipTransaction>();

    public virtual ICollection<StoreOwnerMembership> StoreOwnerMemberships { get; set; } = new List<StoreOwnerMembership>();
}
