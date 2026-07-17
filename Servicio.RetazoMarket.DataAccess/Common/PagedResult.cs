using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicio.RetazoMarket.DataAccess.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                    return 0;

                return (int)Math.Ceiling((double)TotalRecords / PageSize);
            }
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
