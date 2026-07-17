using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IPersonalizacionDataService
    {
        Task<PersonalizacionDataModel?> ObtenerPorIdAsync(int id_opcion, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PersonalizacionDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<PersonalizacionDataModel> CrearAsync(PersonalizacionDataModel model, CancellationToken cancellationToken = default);
        Task<PersonalizacionDataModel?> ActualizarAsync(PersonalizacionDataModel model, CancellationToken cancellationToken = default);
    }
}
