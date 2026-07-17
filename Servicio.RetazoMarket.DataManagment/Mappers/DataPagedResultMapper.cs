using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class DataPagedResultMapper
    {
        public static DataPagedResult<TModel> ToDataPagedResult<TEntity, TModel>(
            PagedResult<TEntity> source,
            Func<TEntity, TModel> mapItem)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(mapItem);

            return new DataPagedResult<TModel>
            {
                Items = source.Items.Select(mapItem).ToList(),
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalRecords = source.TotalRecords
            };
        }
    }
}
