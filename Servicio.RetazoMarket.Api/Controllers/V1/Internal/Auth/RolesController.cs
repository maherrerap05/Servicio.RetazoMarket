using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Rol;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Auth;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.SuperAdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/roles")]
public class RolesController : ControllerBase
{
    private readonly IRolService _service;
    public RolesController(IRolService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RolResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct) =>
        Ok(ApiResponse<IReadOnlyList<RolResponse>>.Ok(await _service.ObtenerTodosAsync(ct), "Consulta exitosa."));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct)
            ?? throw new NotFoundException("El rol no existe.");
        return Ok(ApiResponse<RolResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("nombre/{nombre}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorNombre(string nombre, CancellationToken ct)
    {
        var result = await _service.ObtenerPorNombreAsync(nombre, ct)
            ?? throw new NotFoundException("El rol no existe.");
        return Ok(ApiResponse<RolResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("buscar")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<RolResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buscar([FromQuery] RolFiltroRequest filtro, CancellationToken ct) =>
        Ok(ApiResponse<DataPagedResult<RolResponse>>.Ok(await _service.BuscarAsync(filtro, ct), "Consulta paginada exitosa."));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RolResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearRolRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_rol }, ApiResponse<RolResponse>.Ok(result, "Rol creado exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRolRequest request, CancellationToken ct)
    {
        request.id_rol = id;
        var result = await _service.ActualizarAsync(request, ct)
            ?? throw new NotFoundException("El rol no existe.");
        return Ok(ApiResponse<RolResponse>.Ok(result, "Rol actualizado exitosamente."));
    }
}
