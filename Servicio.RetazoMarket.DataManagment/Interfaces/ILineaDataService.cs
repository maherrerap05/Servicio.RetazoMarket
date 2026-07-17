using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface ILineaDataService
    {
        Task<IReadOnlyList<LineaDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<LineaDataModel?> ObtenerPorIdAsync(int id_linea, CancellationToken cancellationToken = default);
        Task<LineaDataModel?> ObtenerPorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default);
        Task<LineaDataModel> CrearAsync(LineaDataModel model, CancellationToken cancellationToken = default);
        Task<LineaDataModel?> ActualizarAsync(LineaDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_linea, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default);
    }
}
