using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IProveedorDataService
    {
        Task<IReadOnlyList<ProveedorDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<ProveedorDataModel?> ObtenerPorIdAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorDataModel?> ObtenerPorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default);
        Task<ProveedorDataModel?> ObtenerPorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default);
        Task<DataPagedResult<ProveedorDataModel>> BuscarAsync(ProveedorFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ProveedorDataModel> CrearAsync(ProveedorDataModel model, CancellationToken cancellationToken = default);
        Task<ProveedorDataModel?> ActualizarAsync(ProveedorDataModel model, CancellationToken cancellationToken = default);
        Task<bool> EliminarLogicoAsync(int id_proveedor, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default);
    }
}
