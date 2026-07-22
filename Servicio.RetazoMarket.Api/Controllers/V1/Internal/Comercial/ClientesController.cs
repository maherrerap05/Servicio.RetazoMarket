using Asp.Versioning;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Servicio.RetazoMarket.Api.Extensions;using Servicio.RetazoMarket.Api.Models.Common;using Servicio.RetazoMarket.Business.DTOs.Cliente;using Servicio.RetazoMarket.Business.Exceptions;using Servicio.RetazoMarket.Business.Interfaces;using Servicio.RetazoMarket.DataManagment.Models.Common;
namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Comercial;
[ApiController][ApiVersion("1.0")][Authorize(Policy=AuthorizationExtensions.AdministradorPolicy)][Route("api/v{version:apiVersion}/internal/clientes")][ProducesResponseType(typeof(ApiErrorResponse),400)][ProducesResponseType(typeof(ApiErrorResponse),401)][ProducesResponseType(typeof(ApiErrorResponse),403)][ProducesResponseType(typeof(ApiErrorResponse),404)]
public class ClientesController:ControllerBase
{
    private readonly IClienteService _s;public ClientesController(IClienteService s)=>_s=s;
    [HttpGet]public async Task<IActionResult> Todos(CancellationToken ct)=>Ok(ApiResponse<IReadOnlyList<ClienteResponse>>.Ok(await _s.ObtenerTodosAsync(ct),"Consulta exitosa."));
    [HttpGet("{id:int}")]public async Task<IActionResult> PorId(int id,CancellationToken ct){var r=await _s.ObtenerPorIdAsync(id,ct)??throw new NotFoundException("El cliente no existe.");return Ok(ApiResponse<ClienteResponse>.Ok(r));}
    [HttpGet("correo/{correo}")]public async Task<IActionResult> PorCorreo(string correo,CancellationToken ct){var r=await _s.ObtenerPorCorreoAsync(correo,ct)??throw new NotFoundException("El cliente no existe.");return Ok(ApiResponse<ClienteResponse>.Ok(r));}
    [HttpGet("buscar")]public async Task<IActionResult> Buscar([FromQuery]ClienteFiltroRequest f,CancellationToken ct)=>Ok(ApiResponse<DataPagedResult<ClienteResponse>>.Ok(await _s.BuscarAsync(f,ct),"Consulta paginada exitosa."));
    [HttpPost]public async Task<IActionResult> Crear([FromBody]CrearClienteRequest r,CancellationToken ct){var x=await _s.CrearAsync(r,ct);return CreatedAtAction(nameof(PorId),new{version="1.0",id=x.id_cliente},ApiResponse<ClienteResponse>.Ok(x,"Cliente creado exitosamente."));}
    [HttpPut("{id:int}")]public async Task<IActionResult> Actualizar(int id,[FromBody]ActualizarClienteRequest r,CancellationToken ct){r.id_cliente=id;var x=await _s.ActualizarAsync(r,ct)??throw new NotFoundException("El cliente no existe.");return Ok(ApiResponse<ClienteResponse>.Ok(x,"Cliente actualizado exitosamente."));}
    [HttpDelete("{id:int}")]public async Task<IActionResult> Eliminar(int id,CancellationToken ct){if(!await _s.EliminarAsync(id,ct))throw new NotFoundException("El cliente no existe.");return Ok(ApiResponse<bool>.Ok(true,"Cliente eliminado lógicamente."));}
}
