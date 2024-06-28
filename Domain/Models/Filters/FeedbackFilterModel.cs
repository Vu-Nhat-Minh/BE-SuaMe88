namespace Domain.Models.Filters
{
    public class FeedbackFilterModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
