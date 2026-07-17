namespace Servicio.RetazoMarket.DataManagment.Models.Common
{
    public class DataPagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public long TotalRecords { get; set; }

        public int TotalPages => PageSize <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalRecords / PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
