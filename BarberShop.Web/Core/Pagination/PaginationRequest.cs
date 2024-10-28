namespace BarberShop.Web.Core.Pagination
{
    public class PaginationRequest
    {

        public int _page = 1;
        public int _recordsPerPage = 15;
        public int _maxRecordsPerPage = 50;
        

        public string?  Filter { get; set; }

        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value :_page ;
        }

        public int RecordsPerPage 
        {

            get  => _recordsPerPage;

            set => _recordsPerPage = value <= _maxRecordsPerPage ? _recordsPerPage : _maxRecordsPerPage;  
        
        }


    }
}