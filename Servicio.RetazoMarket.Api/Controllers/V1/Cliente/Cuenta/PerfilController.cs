using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.DTOs.Cliente;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Cliente.Cuenta;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.ClientePolicy)]
[Route("api/v{version:apiVersion}/cliente/perfil")]
public class PerfilController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IAutorizacionService _autorizacionService;

    public PerfilController(
        IClienteService clienteService,
        IAutorizacionService autorizacionService)
    {
        _clienteService = clienteService;
        _autorizacionService = autorizacionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obtener(CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        var result = await _clienteService.ObtenerPorIdAsync(actor.id_cliente!.Value, cancellationToken)
            ?? throw new NotFoundException("No se encontró el perfil del cliente autenticado.");

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Consulta del perfil exitosa."));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        [FromBody] ActualizarClienteRequest request,
        CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        var idCliente = actor.id_cliente!.Value;

        var actual = await _clienteService.ObtenerPorIdAsync(idCliente, cancellationToken)
            ?? throw new NotFoundException("No se encontró el perfil del cliente autenticado.");

        request.id_cliente = idCliente;
        request.origen = actual.origen;
        request.cli_estado = actual.cli_estado;

        var result = await _clienteService.ActualizarAsync(request, cancellationToken)
            ?? throw new NotFoundException("No se encontró el perfil que se desea actualizar.");

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Perfil actualizado exitosamente."));
    }

    private ActorContext ObtenerActorCliente()
    {
        var actor = User.ToActorContext();
        _autorizacionService.ExigirClientePropietario(actor, actor.id_cliente ?? 0);
        return actor;
    }
}
