using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface ICategoriaMaterialRepository
    {
        Task<IReadOnlyList<CategoriaMaterialEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<CategoriaMaterialEntity?> ObtenerPorIdAsync(int id_categoria, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialEntity?> ObtenerParaActualizarAsync(int id_categoria, CancellationToken cancellationToken = default);
        Task<CategoriaMaterialEntity?> ObtenerPorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default);
        Task AgregarAsync(CategoriaMaterialEntity categoriaMaterial, CancellationToken cancellationToken = default);
        void Actualizar(CategoriaMaterialEntity categoriaMaterial);
        Task<bool> ExistePorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default);
    }
}
