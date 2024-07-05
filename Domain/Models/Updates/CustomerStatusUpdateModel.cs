namespace Domain.Models.Updates
{
    public class CustomerStatusUpdateModel
    {
        public Guid? CustomerId { get; set; }
        public string? Status { get; set; }
    }
}
