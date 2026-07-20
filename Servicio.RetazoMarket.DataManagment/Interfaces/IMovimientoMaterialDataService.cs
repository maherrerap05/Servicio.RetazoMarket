using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IMovimientoMaterialDataService
    {
        Task<MovimientoMaterialDataModel?> ObtenerPorIdAsync(int id_movimiento, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MovimientoMaterialDataModel>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default);
        Task<DataPagedResult<MovimientoMaterialDataModel>> BuscarAsync(MovimientoMaterialFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<MovimientoMaterialDataModel> CrearAsync(MovimientoMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<MovimientoMaterialDataModel?> CrearConActualizacionStockAsync(
            MovimientoMaterialDataModel model, decimal stock_resultante, CancellationToken cancellationToken = default);
    }
}
