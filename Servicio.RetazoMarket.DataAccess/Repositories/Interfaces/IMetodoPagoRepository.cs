using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Repositories.Interfaces
{
    public interface IMetodoPagoRepository
    {
        Task<IReadOnlyList<MetodoPagoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<MetodoPagoEntity?> ObtenerPorIdAsync(int id_metodo, CancellationToken cancellationToken = default);
        Task<MetodoPagoEntity?> ObtenerParaActualizarAsync(int id_metodo, CancellationToken cancellationToken = default);
        Task<MetodoPagoEntity?> ObtenerPorNombreAsync(string met_nombre, CancellationToken cancellationToken = default);
        Task<MetodoPagoEntity?> ObtenerPorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default);
        Task AgregarAsync(MetodoPagoEntity metodoPago, CancellationToken cancellationToken = default);
        void Actualizar(MetodoPagoEntity metodoPago);
        Task<bool> ExistePorNombreAsync(string met_nombre, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default);
    }
}
