using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IImagenRepository
    {
        Task<ImagenEntity?> ObtenerPorIdAsync(int id_imagen, CancellationToken cancellationToken = default);
        Task<ImagenEntity?> ObtenerParaActualizarAsync(int id_imagen, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ImagenEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task AgregarAsync(ImagenEntity imagen, CancellationToken cancellationToken = default);
        void Actualizar(ImagenEntity imagen);
    }
}
