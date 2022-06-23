namespace RentalAPI.Controllers.ResourceParameters
{
    public class RentablesResourceParameters
    {
        const int maxPageSize = 20;
        private int _pageize = 5;
        public string category { get; set; }
        public string searchQuery { get; set; }

        public int PageNumber { get; set; } = 1;
       
        public int PageSize
        {
            get => _pageize;
            set => _pageize = (value >maxPageSize) ?  maxPageSize: value;
        }

        public string OrderBy { get; set; }
    }
}
