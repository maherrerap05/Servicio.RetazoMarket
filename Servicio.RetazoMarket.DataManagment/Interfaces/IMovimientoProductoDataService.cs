using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IMovimientoProductoDataService
    {
        Task<MovimientoProductoDataModel?> ObtenerPorIdAsync(int id_movimiento2, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MovimientoProductoDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<DataPagedResult<MovimientoProductoDataModel>> BuscarAsync(MovimientoProductoFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<MovimientoProductoDataModel> CrearAsync(MovimientoProductoDataModel model, CancellationToken cancellationToken = default);
        Task<MovimientoProductoDataModel?> CrearConActualizacionStockAsync(
            MovimientoProductoDataModel model, int stock_resultante, CancellationToken cancellationToken = default);
    }
}
