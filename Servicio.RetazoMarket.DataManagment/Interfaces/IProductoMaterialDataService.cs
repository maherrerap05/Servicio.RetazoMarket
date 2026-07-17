using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IProductoMaterialDataService
    {
        Task<ProductoMaterialDataModel?> ObtenerAsync(int id_producto, int id_material, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductoMaterialDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<ProductoMaterialDataModel> CrearAsync(ProductoMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<ProductoMaterialDataModel?> ActualizarAsync(ProductoMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ExisteAsync(int id_producto, int id_material, CancellationToken cancellationToken = default);
    }
}
