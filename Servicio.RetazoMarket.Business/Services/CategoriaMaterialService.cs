using Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;

namespace Servicio.RetazoMarket.Business.Services
{
    public class CategoriaMaterialService : ICategoriaMaterialService
    {
        private readonly ICategoriaMaterialDataService _dataService;

        public CategoriaMaterialService(ICategoriaMaterialDataService dataService) => _dataService = dataService;

        public async Task<IReadOnlyList<CategoriaMaterialResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var models = await _dataService.ObtenerTodosAsync(cancellationToken);
            return models.Select(CategoriaMaterialBusinessMapper.ToResponse).ToList();
        }

        public async Task<CategoriaMaterialResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorIdAsync(id, cancellationToken);
            return model is null ? null : CategoriaMaterialBusinessMapper.ToResponse(model);
        }

        public async Task<CategoriaMaterialResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorNombreAsync(nombre.Trim(), cancellationToken);
            return model is null ? null : CategoriaMaterialBusinessMapper.ToResponse(model);
        }

        public async Task<CategoriaMaterialResponse> CrearAsync(CrearCategoriaMaterialRequest request, CancellationToken cancellationToken = default)
        {
            var errors = CategoriaMaterialValidator.ValidarCreacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            var nombre = request.cat_nombre.Trim();
            if (await _dataService.ExistePorNombreAsync(nombre, cancellationToken))
                throw new ValidationException("Ya existe una categoría de material con ese nombre.");

            var created = await _dataService.CrearAsync(CategoriaMaterialBusinessMapper.ToDataModel(request), cancellationToken);
            return CategoriaMaterialBusinessMapper.ToResponse(created);
        }

        public async Task<CategoriaMaterialResponse?> ActualizarAsync(ActualizarCategoriaMaterialRequest request, CancellationToken cancellationToken = default)
        {
            var errors = CategoriaMaterialValidator.ValidarActualizacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            var existing = await _dataService.ObtenerPorNombreAsync(request.cat_nombre.Trim(), cancellationToken);
            if (existing is not null && existing.id_categoria != request.id_categoria)
                throw new ValidationException("Ya existe otra categoría de material con ese nombre.");

            var updated = await _dataService.ActualizarAsync(CategoriaMaterialBusinessMapper.ToDataModel(request), cancellationToken);
            return updated is null ? null : CategoriaMaterialBusinessMapper.ToResponse(updated);
        }

        public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default) =>
            _dataService.EliminarLogicoAsync(id, cancellationToken);
    }
}
