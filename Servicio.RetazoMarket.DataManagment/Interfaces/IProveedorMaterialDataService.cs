using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IProveedorMaterialDataService
    {
        Task<ProveedorMaterialDataModel?> ObtenerAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProveedorMaterialDataModel>> ObtenerPorProveedorAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProveedorMaterialDataModel>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default);
        Task<ProveedorMaterialDataModel> CrearAsync(ProveedorMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<ProveedorMaterialDataModel?> ActualizarAsync(ProveedorMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ExisteAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default);
    }
}
