using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public int Amount { get; set; }

    public string Receiver { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public bool IsPayment { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public int? Discount { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderVoucher> OrderVouchers { get; set; } = new List<OrderVoucher>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
