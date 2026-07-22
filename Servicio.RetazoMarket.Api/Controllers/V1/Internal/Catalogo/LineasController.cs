using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Linea;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Catalogo;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.SuperAdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/lineas")]
public class LineasController : ControllerBase
{
    private readonly ILineaService _service;
    public LineasController(ILineaService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LineaResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct) => Ok(ApiResponse<IReadOnlyList<LineaResponse>>.Ok(await _service.ObtenerTodosAsync(ct), "Consulta exitosa."));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<LineaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct) ?? throw new NotFoundException("La línea no existe.");
        return Ok(ApiResponse<LineaResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("nombre/{nombre}")]
    [ProducesResponseType(typeof(ApiResponse<LineaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorNombre(string nombre, CancellationToken ct)
    {
        var result = await _service.ObtenerPorNombreAsync(nombre, ct) ?? throw new NotFoundException("La línea no existe.");
        return Ok(ApiResponse<LineaResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("buscar")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<LineaResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buscar([FromQuery] LineaFiltroRequest filtro, CancellationToken ct) => Ok(ApiResponse<DataPagedResult<LineaResponse>>.Ok(await _service.BuscarAsync(filtro, ct), "Consulta paginada exitosa."));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LineaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearLineaRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_linea }, ApiResponse<LineaResponse>.Ok(result, "Línea creada exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<LineaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarLineaRequest request, CancellationToken ct)
    {
        request.id_linea = id;
        var result = await _service.ActualizarAsync(request, ct) ?? throw new NotFoundException("La línea no existe.");
        return Ok(ApiResponse<LineaResponse>.Ok(result, "Línea actualizada exitosamente."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        if (!await _service.EliminarAsync(id, ct)) throw new NotFoundException("La línea no existe.");
        return Ok(ApiResponse<bool>.Ok(true, "Línea eliminada lógicamente."));
    }
}
