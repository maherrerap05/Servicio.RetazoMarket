using Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface ICategoriaMaterialService
    {
        Task<IReadOnlyList<CategoriaMaterialResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<CategoriaMaterialResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialResponse> CrearAsync(CrearCategoriaMaterialRequest request, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialResponse?> ActualizarAsync(ActualizarCategoriaMaterialRequest request, CancellationToken cancellationToken = default);
        Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
    }
}
