using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IFavoritoRepository
    {
        Task<FavoritoEntity?> ObtenerAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<FavoritoEntity>> ObtenerPorClienteAsync(int id_cliente, CancellationToken cancellationToken = default);
        Task AgregarAsync(FavoritoEntity favorito, CancellationToken cancellationToken = default);
        void Eliminar(FavoritoEntity favorito);
        Task<bool> ExisteAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default);
    }
}
