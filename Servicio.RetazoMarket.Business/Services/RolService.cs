using Servicio.RetazoMarket.Business.DTOs.Rol;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolDataService _dataService;

        public RolService(IRolDataService dataService) => _dataService = dataService;

        public async Task<IReadOnlyList<RolResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var models = await _dataService.ObtenerTodosAsync(cancellationToken);
            return models.Select(RolBusinessMapper.ToResponse).ToList();
        }

        public async Task<RolResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorIdAsync(id, cancellationToken);
            return model is null ? null : RolBusinessMapper.ToResponse(model);
        }

        public async Task<RolResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorNombreAsync(NormalizarNombre(nombre), cancellationToken);
            return model is null ? null : RolBusinessMapper.ToResponse(model);
        }

        public async Task<DataPagedResult<RolResponse>> BuscarAsync(RolFiltroRequest filtro, CancellationToken cancellationToken = default)
        {
            var errors = RolValidator.ValidarFiltro(filtro);
            if (errors.Any()) throw new ValidationException("Filtro inválido.", errors);

            var result = await _dataService.BuscarAsync(
                string.IsNullOrWhiteSpace(filtro.nombre_rol) ? null : NormalizarNombre(filtro.nombre_rol),
                filtro.page_number,
                filtro.page_size,
                cancellationToken);

            return new DataPagedResult<RolResponse>
            {
                Items = result.Items.Select(RolBusinessMapper.ToResponse).ToList(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<RolResponse> CrearAsync(CrearRolRequest request, CancellationToken cancellationToken = default)
        {
            var errors = RolValidator.ValidarCreacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            var nombre = NormalizarNombre(request.nombre_rol);
            if (await _dataService.ExistePorNombreAsync(nombre, cancellationToken))
                throw new ValidationException("Ya existe un rol con ese nombre.");

            var created = await _dataService.CrearAsync(RolBusinessMapper.ToDataModel(request), cancellationToken);
            return RolBusinessMapper.ToResponse(created);
        }

        public async Task<RolResponse?> ActualizarAsync(ActualizarRolRequest request, CancellationToken cancellationToken = default)
        {
            var errors = RolValidator.ValidarActualizacion(request);
            if (errors.Any()) throw new ValidationException("Solicitud inválida.", errors);

            var existing = await _dataService.ObtenerPorNombreAsync(NormalizarNombre(request.nombre_rol), cancellationToken);
            if (existing is not null && existing.id_rol != request.id_rol)
                throw new ValidationException("Ya existe otro rol con ese nombre.");

            var updated = await _dataService.ActualizarAsync(RolBusinessMapper.ToDataModel(request), cancellationToken);
            return updated is null ? null : RolBusinessMapper.ToResponse(updated);
        }

        private static string NormalizarNombre(string nombre) => nombre.Trim().ToUpperInvariant();
    }
}
