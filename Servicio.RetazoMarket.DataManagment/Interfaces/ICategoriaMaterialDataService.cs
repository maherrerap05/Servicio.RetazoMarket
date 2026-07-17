using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface ICategoriaMaterialDataService
    {
        Task<IReadOnlyList<CategoriaMaterialDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<CategoriaMaterialDataModel?> ObtenerPorIdAsync(int id_categoria, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialDataModel?> ObtenerPorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialDataModel> CrearAsync(CategoriaMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialDataModel?> ActualizarAsync(CategoriaMaterialDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_categoria, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default);
    }
}
