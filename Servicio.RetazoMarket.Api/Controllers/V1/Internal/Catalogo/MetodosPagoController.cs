using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.MetodoPago;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Catalogo;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.SuperAdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/metodos-pago")]
public class MetodosPagoController : ControllerBase
{
    private readonly IMetodoPagoService _service;
    public MetodosPagoController(IMetodoPagoService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MetodoPagoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct) => Ok(ApiResponse<IReadOnlyList<MetodoPagoResponse>>.Ok(await _service.ObtenerTodosAsync(ct), "Consulta exitosa."));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MetodoPagoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct) ?? throw new NotFoundException("El método de pago no existe.");
        return Ok(ApiResponse<MetodoPagoResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("nombre/{nombre}")]
    public async Task<IActionResult> ObtenerPorNombre(string nombre, CancellationToken ct)
    {
        var result = await _service.ObtenerPorNombreAsync(nombre, ct) ?? throw new NotFoundException("El método de pago no existe.");
        return Ok(ApiResponse<MetodoPagoResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("codigo-sri/{codigoSri}")]
    public async Task<IActionResult> ObtenerPorCodigoSri(string codigoSri, CancellationToken ct)
    {
        var result = await _service.ObtenerPorCodigoSriAsync(codigoSri, ct) ?? throw new NotFoundException("El método de pago no existe.");
        return Ok(ApiResponse<MetodoPagoResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MetodoPagoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearMetodoPagoRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_metodo }, ApiResponse<MetodoPagoResponse>.Ok(result, "Método de pago creado exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MetodoPagoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMetodoPagoRequest request, CancellationToken ct)
    {
        request.id_metodo = id;
        var result = await _service.ActualizarAsync(request, ct) ?? throw new NotFoundException("El método de pago no existe.");
        return Ok(ApiResponse<MetodoPagoResponse>.Ok(result, "Método de pago actualizado exitosamente."));
    }
}
