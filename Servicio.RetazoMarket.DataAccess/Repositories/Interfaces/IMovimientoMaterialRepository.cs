using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IMovimientoMaterialRepository
    {
        Task<MovimientoMaterialEntity?> ObtenerPorIdAsync(int id_movimiento, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MovimientoMaterialEntity>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default);
        Task AgregarAsync(MovimientoMaterialEntity movimiento, CancellationToken cancellationToken = default);
    }
}
