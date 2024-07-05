namespace Domain.Models.Views
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public CustomerViewModel Customer { get; set; } = null!;

        public int Amount { get; set; }

        public string Receiver { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string PaymentMethod { get; set; } = null!;

        public bool IsPayment { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public int? Discount { get; set; }

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }
}
