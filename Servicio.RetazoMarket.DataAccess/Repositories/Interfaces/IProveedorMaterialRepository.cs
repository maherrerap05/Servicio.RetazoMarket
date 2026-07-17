using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IProveedorMaterialRepository
    {
        Task<ProveedorMaterialEntity?> ObtenerAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorMaterialEntity?> ObtenerParaActualizarAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProveedorMaterialEntity>> ObtenerPorProveedorAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProveedorMaterialEntity>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default);
        Task AgregarAsync(ProveedorMaterialEntity proveedorMaterial, CancellationToken cancellationToken = default);
        void Actualizar(ProveedorMaterialEntity proveedorMaterial);
        Task<bool> ExisteAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default);
    }
}
