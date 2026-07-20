using Servicio.RetazoMarket.Business.DTOs.MetodoPago;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;

namespace Servicio.RetazoMarket.Business.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly IMetodoPagoDataService _dataService;

        public MetodoPagoService(IMetodoPagoDataService dataService) => _dataService = dataService;

        public async Task<IReadOnlyList<MetodoPagoResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var models = await _dataService.ObtenerTodosAsync(cancellationToken);
            return models.Select(MetodoPagoBusinessMapper.ToResponse).ToList();
        }

        public async Task<MetodoPagoResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorIdAsync(id, cancellationToken);
            return model is null ? null : MetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<MetodoPagoResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorNombreAsync(nombre.Trim(), cancellationToken);
            return model is null ? null : MetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<MetodoPagoResponse?> ObtenerPorCodigoSriAsync(string codigoSri, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorCodigoSriAsync(codigoSri.Trim(), cancellationToken);
            return model is null ? null : MetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<MetodoPagoResponse> CrearAsync(CrearMetodoPagoRequest request, CancellationToken cancellationToken = default)
        {
            var errors = MetodoPagoValidator.ValidarCreacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            await ValidarDuplicadosAsync(request.met_nombre, request.codigo_sri, null, cancellationToken);
            var created = await _dataService.CrearAsync(MetodoPagoBusinessMapper.ToDataModel(request), cancellationToken);
            return MetodoPagoBusinessMapper.ToResponse(created);
        }

        public async Task<MetodoPagoResponse?> ActualizarAsync(ActualizarMetodoPagoRequest request, CancellationToken cancellationToken = default)
        {
            var errors = MetodoPagoValidator.ValidarActualizacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            await ValidarDuplicadosAsync(request.met_nombre, request.codigo_sri, request.id_metodo, cancellationToken);
            var updated = await _dataService.ActualizarAsync(MetodoPagoBusinessMapper.ToDataModel(request), cancellationToken);
            return updated is null ? null : MetodoPagoBusinessMapper.ToResponse(updated);
        }

        private async Task ValidarDuplicadosAsync(string nombre, string codigoSri, int? idActual, CancellationToken cancellationToken)
        {
            var porNombre = await _dataService.ObtenerPorNombreAsync(nombre.Trim(), cancellationToken);
            if (porNombre is not null && porNombre.id_metodo != idActual)
                throw new ValidationException("Ya existe un método de pago con ese nombre.");

            var porCodigo = await _dataService.ObtenerPorCodigoSriAsync(codigoSri.Trim(), cancellationToken);
            if (porCodigo is not null && porCodigo.id_metodo != idActual)
                throw new ValidationException("Ya existe un método de pago con ese código SRI.");
        }
    }
}
