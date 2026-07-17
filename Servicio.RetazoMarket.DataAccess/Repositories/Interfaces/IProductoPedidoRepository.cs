using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IProductoPedidoRepository
    {
        Task<ProductoPedidoEntity?> ObtenerAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default);
        Task<ProductoPedidoEntity?> ObtenerParaActualizarAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductoPedidoEntity>> ObtenerPorPedidoAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task AgregarAsync(ProductoPedidoEntity detalle, CancellationToken cancellationToken = default);
        void Actualizar(ProductoPedidoEntity detalle);
        Task<bool> ExisteAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default);
    }
}
