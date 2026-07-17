using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<IReadOnlyList<ProductoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<ProductoEntity?> ObtenerPorIdAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<ProductoEntity?> ObtenerParaActualizarAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<ProductoEntity?> ObtenerPorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default);
        Task AgregarAsync(ProductoEntity producto, CancellationToken cancellationToken = default);
        void Actualizar(ProductoEntity producto);
        Task<bool> ExistePorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default);
    }
}
