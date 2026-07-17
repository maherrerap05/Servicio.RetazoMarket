using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface ILineaRepository
    {
        Task<IReadOnlyList<LineaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<LineaEntity?> ObtenerPorIdAsync(int id_linea, CancellationToken cancellationToken = default);
        Task<LineaEntity?> ObtenerParaActualizarAsync(int id_linea, CancellationToken cancellationToken = default);
        Task<LineaEntity?> ObtenerPorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default);
        Task AgregarAsync(LineaEntity linea, CancellationToken cancellationToken = default);
        void Actualizar(LineaEntity linea);
        Task<bool> ExistePorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default);
    }
}
