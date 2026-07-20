using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IDescuentoDataService
    {
        Task<DescuentoDataModel?> ObtenerPorIdAsync(
            int id_descuento,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DescuentoDataModel>> ObtenerTodosAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DescuentoDataModel>> ObtenerPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<DescuentoDataModel>> BuscarAsync(
            DescuentoFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DescuentoDataModel>> ObtenerActivosPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default);

        Task<DescuentoDataModel?> ObtenerAplicableAsync(
            int id_producto,
            int cantidad_comprada,
            CancellationToken cancellationToken = default);

        Task<DescuentoDataModel> CrearAsync(
            DescuentoDataModel model,
            CancellationToken cancellationToken = default);

        Task<DescuentoDataModel?> ActualizarAsync(
            DescuentoDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> InactivarAsync(
            int id_descuento,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorProductoYCantidadMinimaAsync(
            int id_producto,
            int cantidad_minima,
            int? id_descuento_excluir = null,
            CancellationToken cancellationToken = default);
    }
}
