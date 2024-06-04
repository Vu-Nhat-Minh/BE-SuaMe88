using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StoreOwnerMembership
{
    public Guid Id { get; set; }

    public Guid StoreOwnerId { get; set; }

    public Guid MembershipId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Membership Membership { get; set; } = null!;

    public virtual StoreOwner StoreOwner { get; set; } = null!;
}
