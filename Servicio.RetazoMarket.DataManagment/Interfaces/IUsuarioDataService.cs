using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IUsuarioDataService
    {
        Task<IReadOnlyList<UsuarioDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UsuarioDataModel>> ObtenerActivosAsync(CancellationToken cancellationToken = default);
        Task<UsuarioDataModel?> ObtenerPorIdAsync(int id_usuario, CancellationToken cancellationToken = default);
        Task<UsuarioDataModel?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
        Task<DataPagedResult<UsuarioDataModel>> BuscarAsync(UsuarioFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<UsuarioDataModel> CrearAsync(UsuarioDataModel model, CancellationToken cancellationToken = default);
        Task<UsuarioDataModel?> ActualizarAsync(UsuarioDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_usuario, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    }
}
