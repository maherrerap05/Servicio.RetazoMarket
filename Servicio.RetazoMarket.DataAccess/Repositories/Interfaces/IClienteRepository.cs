using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        // =========================
        // CONSULTAS
        // =========================
        Task<IReadOnlyList<ClienteEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<ClienteEntity?> ObtenerPorIdAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task<ClienteEntity?> ObtenerParaActualizarAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task<ClienteEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);

        // =========================
        // COMANDOS
        // =========================
        Task AgregarAsync(ClienteEntity cliente, CancellationToken cancellationToken = default);
        void Actualizar(ClienteEntity cliente);

        // =========================
        // VALIDACIONES
        // =========================
        Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    }
}
