using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Usuario;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Auth;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.SuperAdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;
    public UsuariosController(IUsuarioService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UsuarioResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct) =>
        Ok(ApiResponse<IReadOnlyList<UsuarioResponse>>.Ok(await _service.ObtenerTodosAsync(ct), "Consulta exitosa."));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct)
            ?? throw new NotFoundException("El usuario no existe.");
        return Ok(ApiResponse<UsuarioResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("correo/{correo}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorCorreo(string correo, CancellationToken ct)
    {
        var result = await _service.ObtenerPorCorreoAsync(correo, ct)
            ?? throw new NotFoundException("El usuario no existe.");
        return Ok(ApiResponse<UsuarioResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("buscar")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<UsuarioResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buscar([FromQuery] UsuarioFiltroRequest filtro, CancellationToken ct) =>
        Ok(ApiResponse<DataPagedResult<UsuarioResponse>>.Ok(await _service.BuscarAsync(filtro, ct), "Consulta paginada exitosa."));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_usuario }, ApiResponse<UsuarioResponse>.Ok(result, "Usuario creado exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioRequest request, CancellationToken ct)
    {
        request.id_usuario = id;
        var result = await _service.ActualizarAsync(request, ct)
            ?? throw new NotFoundException("El usuario no existe.");
        return Ok(ApiResponse<UsuarioResponse>.Ok(result, "Usuario actualizado exitosamente."));
    }

    [HttpPut("{id:int}/contrasena")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarContrasena(int id, [FromBody] CambiarContrasenaRequest request, CancellationToken ct)
    {
        request.id_usuario = id;
        if (!await _service.CambiarContrasenaAsync(request, ct))
            throw new NotFoundException("El usuario no existe.");
        return Ok(ApiResponse<bool>.Ok(true, "Contraseña actualizada exitosamente."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        if (!await _service.EliminarAsync(id, ct))
            throw new NotFoundException("El usuario no existe.");
        return Ok(ApiResponse<bool>.Ok(true, "Usuario eliminado lógicamente."));
    }
}
