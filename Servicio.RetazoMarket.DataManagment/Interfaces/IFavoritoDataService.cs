using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IFavoritoDataService
    {
        Task<FavoritoDataModel?> ObtenerAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<FavoritoDataModel>> ObtenerPorClienteAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task<DataPagedResult<FavoritoDataModel>> BuscarPorClienteAsync(FavoritoFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<FavoritoDataModel> CrearAsync(FavoritoDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default);
        Task<bool> ExisteAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default);
    }
}
