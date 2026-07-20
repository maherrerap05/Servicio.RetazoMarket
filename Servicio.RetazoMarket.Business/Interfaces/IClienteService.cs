using Servicio.RetazoMarket.Business.DTOs.Cliente;
using Servicio.RetazoMarket.DataManagment.Models.Common;
namespace Servicio.RetazoMarket.Business.Interfaces;
public interface IClienteService
{
    Task<IReadOnlyList<ClienteResponse>> ObtenerTodosAsync(CancellationToken cancellationToken=default);
    Task<ClienteResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken=default);
    Task<ClienteResponse?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken=default);
    Task<DataPagedResult<ClienteResponse>> BuscarAsync(ClienteFiltroRequest filtro, CancellationToken cancellationToken=default);
    Task<ClienteResponse> CrearAsync(CrearClienteRequest request, CancellationToken cancellationToken=default);
    Task<ClienteResponse?> ActualizarAsync(ActualizarClienteRequest request, CancellationToken cancellationToken=default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken=default);
}
