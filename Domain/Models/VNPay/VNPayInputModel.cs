namespace Domain.Models.VNPay
{
    public class VnPayInputModel
    {
        public int Amount { get; set; }
        public Guid OrderId { get; set; }
    }
}
