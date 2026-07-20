using Servicio.RetazoMarket.Business.DTOs.Linea;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface ILineaService
    {
        Task<LineaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<LineaResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<LineaResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<DataPagedResult<LineaResponse>> BuscarAsync(LineaFiltroRequest filtro, CancellationToken cancellationToken = default);
        Task<LineaResponse> CrearAsync(CrearLineaRequest request, CancellationToken cancellationToken = default);
        Task<LineaResponse?> ActualizarAsync(ActualizarLineaRequest request, CancellationToken cancellationToken = default);
        Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
    }
}
