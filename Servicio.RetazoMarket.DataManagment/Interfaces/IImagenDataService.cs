using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IImagenDataService
    {
        Task<ImagenDataModel?> ObtenerPorIdAsync(int id_imagen, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ImagenDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<ImagenDataModel> CrearAsync(ImagenDataModel model, CancellationToken cancellationToken = default);
        Task<ImagenDataModel?> ActualizarAsync(ImagenDataModel model, CancellationToken cancellationToken = default);
    }
}
