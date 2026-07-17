using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IProductoMaterialRepository
    {
        Task<ProductoMaterialEntity?> ObtenerAsync(int id_producto, int id_material, CancellationToken cancellationToken = default);
        Task<ProductoMaterialEntity?> ObtenerParaActualizarAsync(int id_producto, int id_material, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductoMaterialEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task AgregarAsync(ProductoMaterialEntity productoMaterial, CancellationToken cancellationToken = default);
        void Actualizar(ProductoMaterialEntity productoMaterial);
        Task<bool> ExisteAsync(int id_producto, int id_material, CancellationToken cancellationToken = default);
    }
}
