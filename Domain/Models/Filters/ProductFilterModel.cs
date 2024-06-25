using Domain.Models.Pagination;

namespace Domain.Models.Filters
{
    public class ProductFilterModel
    {
        public string? Search { get; set; }
        public ICollection<Guid>? Categories { get; set; }
    }
}
