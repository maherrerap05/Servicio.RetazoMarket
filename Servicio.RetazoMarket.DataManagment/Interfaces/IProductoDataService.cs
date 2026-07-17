using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IProductoDataService
    {
        Task<IReadOnlyList<ProductoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<ProductoDataModel?> ObtenerPorIdAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<ProductoDataModel?> ObtenerPorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default);
        Task<DataPagedResult<ProductoDataModel>> BuscarAsync(ProductoFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ProductoDataModel> CrearAsync(ProductoDataModel model, CancellationToken cancellationToken = default);
        Task<ProductoDataModel?> ActualizarAsync(ProductoDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_producto, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default);
    }
}
