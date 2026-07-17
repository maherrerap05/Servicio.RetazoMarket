using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IPersonalizacionRepository
    {
        Task<PersonalizacionEntity?> ObtenerPorIdAsync(int id_opcion, CancellationToken cancellationToken = default);
        Task<PersonalizacionEntity?> ObtenerParaActualizarAsync(int id_opcion, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PersonalizacionEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task AgregarAsync(PersonalizacionEntity personalizacion, CancellationToken cancellationToken = default);
        void Actualizar(PersonalizacionEntity personalizacion);
    }
}
