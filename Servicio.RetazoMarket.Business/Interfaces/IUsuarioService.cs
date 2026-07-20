using Servicio.RetazoMarket.Business.DTOs.Usuario;
using Servicio.RetazoMarket.DataManagment.Models.Common;
namespace Servicio.RetazoMarket.Business.Interfaces;
public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioResponse>> ObtenerTodosAsync(CancellationToken cancellationToken=default);
    Task<UsuarioResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken=default);
    Task<UsuarioResponse?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken=default);
    Task<DataPagedResult<UsuarioResponse>> BuscarAsync(UsuarioFiltroRequest filtro, CancellationToken cancellationToken=default);
    Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request, CancellationToken cancellationToken=default);
    Task<UsuarioResponse?> ActualizarAsync(ActualizarUsuarioRequest request, CancellationToken cancellationToken=default);
    Task<bool> CambiarContrasenaAsync(CambiarContrasenaRequest request, CancellationToken cancellationToken=default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken=default);
}
