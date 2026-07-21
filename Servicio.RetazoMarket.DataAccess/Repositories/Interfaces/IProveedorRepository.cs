using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IProveedorRepository
    {
        Task<IReadOnlyList<ProveedorEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<ProveedorEntity?> ObtenerPorIdAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorEntity?> ObtenerParaActualizarAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorEntity?> ObtenerPorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorEntity?> ObtenerPorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default);
        Task AgregarAsync(ProveedorEntity proveedor, CancellationToken cancellationToken = default);
        void Actualizar(ProveedorEntity proveedor);
        Task<bool> ExistePorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default);
    }
}
