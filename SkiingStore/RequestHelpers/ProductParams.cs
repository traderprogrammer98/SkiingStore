namespace SkiingStore.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string SortBy { get; set; }
        public string Search { get; set; }
        public string Brands { get; set; }
        public string Types { get; set; }

    }
}
