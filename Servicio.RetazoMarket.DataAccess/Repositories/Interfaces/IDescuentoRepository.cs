using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IDescuentoRepository
    {
        Task<IReadOnlyList<DescuentoEntity>> ObtenerTodosAsync(
            CancellationToken cancellationToken = default);

        Task<DescuentoEntity?> ObtenerPorIdAsync(
            int id_descuento,
            CancellationToken cancellationToken = default);

        Task<DescuentoEntity?> ObtenerParaActualizarAsync(
            int id_descuento,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DescuentoEntity>> ObtenerPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default);

        Task AgregarAsync(
            DescuentoEntity descuento,
            CancellationToken cancellationToken = default);

        void Actualizar(DescuentoEntity descuento);

        Task<bool> ExistePorProductoYCantidadMinimaAsync(
            int id_producto,
            int cantidad_minima,
            int? id_descuento_excluir = null,
            CancellationToken cancellationToken = default);
    }
}
