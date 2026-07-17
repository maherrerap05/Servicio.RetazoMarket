using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IPedidoDataService
    {
        Task<IReadOnlyList<PedidoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<PedidoDataModel?> ObtenerPorIdAsync(int id_pedido, CancellationToken cancellationToken = default);
        Task<DataPagedResult<PedidoDataModel>> BuscarAsync(PedidoFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<PedidoDataModel> CrearAsync(PedidoDataModel model, CancellationToken cancellationToken = default);
        Task<PedidoDataModel?> ActualizarAsync(PedidoDataModel model, CancellationToken cancellationToken = default);
        Task<PagoSimuladoDataResult> PagarAsync(int id_pedido, CancellationToken cancellationToken = default);
    }
}
