using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Personalizacion;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Catalogo;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.SuperAdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/personalizaciones")]
public class PersonalizacionesController : ControllerBase
{
    private readonly IPersonalizacionService _service;
    public PersonalizacionesController(IPersonalizacionService service) => _service = service;

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PersonalizacionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct) ?? throw new NotFoundException("La personalización no existe.");
        return Ok(ApiResponse<PersonalizacionResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("producto/{idProducto:int}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PersonalizacionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorProducto(int idProducto, CancellationToken ct) => Ok(ApiResponse<IReadOnlyList<PersonalizacionResponse>>.Ok(await _service.ObtenerPorProductoAsync(idProducto, ct), "Consulta exitosa."));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PersonalizacionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearPersonalizacionRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_opcion }, ApiResponse<PersonalizacionResponse>.Ok(result, "Personalización creada exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PersonalizacionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarPersonalizacionRequest request, CancellationToken ct)
    {
        request.id_opcion = id;
        var result = await _service.ActualizarAsync(request, ct) ?? throw new NotFoundException("La personalización no existe.");
        return Ok(ApiResponse<PersonalizacionResponse>.Ok(result, "Personalización actualizada exitosamente."));
    }
}
