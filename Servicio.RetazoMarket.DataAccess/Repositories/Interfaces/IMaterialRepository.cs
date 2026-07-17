using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IReadOnlyList<MaterialEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<MaterialEntity?> ObtenerPorIdAsync(int id_material, CancellationToken cancellationToken = default);
        Task<MaterialEntity?> ObtenerParaActualizarAsync(int id_material, CancellationToken cancellationToken = default);
        Task<MaterialEntity?> ObtenerPorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default);
        Task AgregarAsync(MaterialEntity material, CancellationToken cancellationToken = default);
        void Actualizar(MaterialEntity material);
        Task<bool> ExistePorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default);
    }
}
