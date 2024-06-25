namespace Domain.Models.Views
{
    public class ProductCategoryViewModel
    {
        public Guid Id { get; set; }

        public CategoryViewModel Category { get; set; } = null!;
    }
}
