using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IProductoPedidoDataService
    {
        Task<ProductoPedidoDataModel?> ObtenerAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductoPedidoDataModel>> ObtenerPorPedidoAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task<ProductoPedidoDataModel> CrearAsync(ProductoPedidoDataModel model, CancellationToken cancellationToken = default);
        Task<ProductoPedidoDataModel?> ActualizarAsync(ProductoPedidoDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ExisteAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default);
    }
}
