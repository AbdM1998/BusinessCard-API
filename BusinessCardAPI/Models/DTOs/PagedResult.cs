namespace BusinessCardAPI.Models.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Cards { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
