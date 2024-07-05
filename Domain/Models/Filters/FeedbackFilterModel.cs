namespace Domain.Models.Filters
{
    public class FeedbackFilterModel
    {
        public Guid? ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
