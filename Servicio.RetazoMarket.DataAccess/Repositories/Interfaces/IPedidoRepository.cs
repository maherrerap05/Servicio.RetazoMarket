using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IReadOnlyList<PedidoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<PedidoEntity?> ObtenerPorIdAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task<PedidoEntity?> ObtenerParaActualizarAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task<PedidoEntity?> ObtenerParaPagarAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task AgregarAsync(PedidoEntity pedido, CancellationToken cancellationToken = default);
        void Actualizar(PedidoEntity pedido);
    }
}
