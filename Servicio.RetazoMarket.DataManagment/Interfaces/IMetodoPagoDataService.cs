using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IMetodoPagoDataService
    {
        Task<IReadOnlyList<MetodoPagoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<MetodoPagoDataModel?> ObtenerPorIdAsync(int id_metodo, CancellationToken cancellationToken = default);
        Task<MetodoPagoDataModel?> ObtenerPorNombreAsync(string met_nombre, CancellationToken cancellationToken = default);
        Task<MetodoPagoDataModel?> ObtenerPorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default);
        Task<MetodoPagoDataModel> CrearAsync(MetodoPagoDataModel model, CancellationToken cancellationToken = default);
        Task<MetodoPagoDataModel?> ActualizarAsync(MetodoPagoDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string met_nombre, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default);
    }
}
