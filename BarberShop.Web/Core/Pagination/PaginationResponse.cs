namespace BarberShop.Web.Core.Pagination
{
    public class PaginationResponse
    {

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int RecordsPerPage { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public int VisiblePages => 5;

        public string? Filter { get; set; }



    }
}
