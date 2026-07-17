using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IRolDataService
    {
        Task<IReadOnlyList<RolDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<RolDataModel?> ObtenerPorIdAsync(int id_rol, CancellationToken cancellationToken = default);
        Task<RolDataModel?> ObtenerPorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default);
        Task<DataPagedResult<RolDataModel>> BuscarAsync(string? nombre_rol, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<RolDataModel> CrearAsync(RolDataModel model, CancellationToken cancellationToken = default);
        Task<RolDataModel?> ActualizarAsync(RolDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default);
    }
}
