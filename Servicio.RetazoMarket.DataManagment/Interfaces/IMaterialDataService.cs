using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IMaterialDataService
    {
        Task<IReadOnlyList<MaterialDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<MaterialDataModel?> ObtenerPorIdAsync(int id_material, CancellationToken cancellationToken = default);
        Task<MaterialDataModel?> ObtenerPorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default);
        Task<DataPagedResult<MaterialDataModel>> BuscarAsync(MaterialFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<MaterialDataModel> CrearAsync(MaterialDataModel model, CancellationToken cancellationToken = default);
        Task<MaterialDataModel?> ActualizarAsync(MaterialDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_material, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default);
    }
}
