using Servicio.RetazoMarket.Business.DTOs.Linea;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services
{
    public class LineaService : ILineaService
    {
        private readonly ILineaDataService _dataService;

        public LineaService(ILineaDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<LineaResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorIdAsync(id, cancellationToken);
            return model is null ? null : LineaBusinessMapper.ToResponse(model);
        }

        public async Task<LineaResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            var model = await _dataService.ObtenerPorNombreAsync(nombre.Trim(), cancellationToken);
            return model is null ? null : LineaBusinessMapper.ToResponse(model);
        }

        public async Task<IReadOnlyList<LineaResponse>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var list = await _dataService.ObtenerTodosAsync(cancellationToken);
            return list.Select(LineaBusinessMapper.ToResponse).ToList();
        }

        public async Task<DataPagedResult<LineaResponse>> BuscarAsync(
            LineaFiltroRequest filtro,
            CancellationToken cancellationToken = default)
        {
            var errors = LineaValidator.ValidarFiltro(filtro);
            if (errors.Any())
                throw new ValidationException("Filtro inválido.", errors);

            var data = await _dataService.ObtenerTodosAsync(cancellationToken);
            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.lin_nombre))
                query = query.Where(x => x.lin_nombre != null &&
                    x.lin_nombre.Contains(filtro.lin_nombre.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filtro.lin_estado))
                query = query.Where(x => x.lin_estado == filtro.lin_estado);

            var total = query.Count();
            var items = query
                .OrderBy(x => x.id_linea)
                .Skip((filtro.page_number - 1) * filtro.page_size)
                .Take(filtro.page_size)
                .ToList();

            return new DataPagedResult<LineaResponse>
            {
                Items = items.Select(LineaBusinessMapper.ToResponse).ToList(),
                PageNumber = filtro.page_number,
                PageSize = filtro.page_size,
                TotalRecords = total
            };
        }

        public async Task<LineaResponse> CrearAsync(
            CrearLineaRequest request,
            CancellationToken cancellationToken = default)
        {
            var errors = LineaValidator.ValidarCreacion(request);
            if (errors.Any())
                throw new ValidationException("Solicitud inválida.", errors);

            var nombre = request.lin_nombre.Trim();
            if (await _dataService.ExistePorNombreAsync(nombre, cancellationToken))
                throw new ValidationException("Ya existe una línea con ese nombre.");

            var model = LineaBusinessMapper.ToDataModel(request);
            var created = await _dataService.CrearAsync(model, cancellationToken);
            return LineaBusinessMapper.ToResponse(created);
        }

        public async Task<LineaResponse?> ActualizarAsync(
            ActualizarLineaRequest request,
            CancellationToken cancellationToken = default)
        {
            var errors = LineaValidator.ValidarActualizacion(request);
            if (errors.Any())
                throw new ValidationException("Solicitud inválida.", errors);

            var porNombre = await _dataService.ObtenerPorNombreAsync(request.lin_nombre.Trim(), cancellationToken);
            if (porNombre is not null && porNombre.id_linea != request.id_linea)
                throw new ValidationException("Ya existe otra línea con ese nombre.");

            var model = LineaBusinessMapper.ToDataModel(request);
            var updated = await _dataService.ActualizarAsync(model, cancellationToken);
            return updated is null ? null : LineaBusinessMapper.ToResponse(updated);
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dataService.EliminarLogicoAsync(id, cancellationToken);
        }
    }
}
