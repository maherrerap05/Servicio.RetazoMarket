using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IMovimientoProductoRepository
    {
        Task<MovimientoProductoEntity?> ObtenerPorIdAsync(int id_movimiento2, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MovimientoProductoEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task AgregarAsync(MovimientoProductoEntity movimiento, CancellationToken cancellationToken = default);
    }
}
