using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IRolRepository
    {
        // =========================
        // CONSULTAS
        // =========================
        Task<IReadOnlyList<RolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerPorIdAsync(int id_rol, CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerParaActualizarAsync(int id_rol, CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerPorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default);

        // =========================
        // COMANDOS
        // =========================
        Task AgregarAsync(RolEntity rol, CancellationToken cancellationToken = default);
        void Actualizar(RolEntity rol);

        // =========================
        // VALIDACIONES
        // =========================
        Task<bool> ExistePorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default);
    }
}
