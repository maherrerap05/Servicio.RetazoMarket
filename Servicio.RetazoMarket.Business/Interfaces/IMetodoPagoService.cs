using Servicio.RetazoMarket.Business.DTOs.MetodoPago;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface IMetodoPagoService
    {
        Task<IReadOnlyList<MetodoPagoResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<MetodoPagoResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<MetodoPagoResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task<MetodoPagoResponse?> ObtenerPorCodigoSriAsync(string codigoSri, CancellationToken cancellationToken = default);
        Task<MetodoPagoResponse> CrearAsync(CrearMetodoPagoRequest request, CancellationToken cancellationToken = default);
        Task<MetodoPagoResponse?> ActualizarAsync(ActualizarMetodoPagoRequest request, CancellationToken cancellationToken = default);
    }
}
