using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        // =========================
        // CONSULTAS
        // =========================
        Task<IReadOnlyList<UsuarioEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<UsuarioEntity?> ObtenerPorIdAsync(int id_usuario, CancellationToken cancellationToken = default);
        Task<UsuarioEntity?> ObtenerParaActualizarAsync(int id_usuario, CancellationToken cancellationToken = default);
        Task<UsuarioEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);

        // =========================
        // COMANDOS
        // =========================
        Task AgregarAsync(UsuarioEntity usuario, CancellationToken cancellationToken = default);
        void Actualizar(UsuarioEntity usuario);

        // =========================
        // VALIDACIONES
        // =========================
        Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    }
}
