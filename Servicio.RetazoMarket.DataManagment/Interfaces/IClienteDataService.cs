using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IClienteDataService
    {
        Task<IReadOnlyList<ClienteDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ClienteDataModel>> ObtenerActivosAsync(CancellationToken cancellationToken = default);
        Task<ClienteDataModel?> ObtenerPorIdAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task<ClienteDataModel?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
        Task<DataPagedResult<ClienteDataModel>> BuscarAsync(ClienteFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ClienteDataModel> CrearAsync(ClienteDataModel model, CancellationToken cancellationToken = default);
        Task<ClienteDataModel?> ActualizarAsync(ClienteDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    }
}
