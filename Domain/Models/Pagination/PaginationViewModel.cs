namespace Domain.Models.Pagination
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int totalRows { get; set; }
    }
}
