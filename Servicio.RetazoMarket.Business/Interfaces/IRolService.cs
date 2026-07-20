using Servicio.RetazoMarket.Business.DTOs.Rol;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface IRolService
    {
        Task<IReadOnlyList<RolResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<RolResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RolResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task<DataPagedResult<RolResponse>> BuscarAsync(RolFiltroRequest filtro, CancellationToken cancellationToken = default);
        Task<RolResponse> CrearAsync(CrearRolRequest request, CancellationToken cancellationToken = default);
        Task<RolResponse?> ActualizarAsync(ActualizarRolRequest request, CancellationToken cancellationToken = default);
    }
}
