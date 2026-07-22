using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.DTOs.Pedido;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Cliente.Pedidos;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.ClientePolicy)]
[Route("api/v{version:apiVersion}/cliente/pedidos")]
public class PedidosClienteController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly IAutorizacionService _autorizacionService;

    public PedidosClienteController(
        IPedidoService pedidoService,
        IAutorizacionService autorizacionService)
    {
        _pedidoService = pedidoService;
        _autorizacionService = autorizacionService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<DataPagedResult<PedidoResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Buscar(
        [FromQuery] PedidoFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        filtro.id_cliente = idCliente;

        var result = await _pedidoService.BuscarAsync(filtro, cancellationToken);

        return Ok(ApiResponse<DataPagedResult<PedidoResponse>>.Ok(
            result,
            "Consulta de pedidos exitosa."));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        var result = await ObtenerPedidoPropioAsync(actor, id, cancellationToken);

        return Ok(ApiResponse<PedidoResponse>.Ok(result, "Consulta del pedido exitosa."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PedidoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        request.id_cliente = actor.id_cliente!.Value;

        var result = await _pedidoService.CrearAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { version = "1.0", id = result.id_pedido },
            ApiResponse<PedidoResponse>.Ok(result, "Pedido creado exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        await ObtenerPedidoPropioAsync(actor, id, cancellationToken);

        request.id_pedido = id;
        request.id_cliente = actor.id_cliente!.Value;

        var result = await _pedidoService.ActualizarAsync(request, cancellationToken)
            ?? throw new NotFoundException("No se encontró el pedido que se desea actualizar.");

        return Ok(ApiResponse<PedidoResponse>.Ok(result, "Pedido actualizado exitosamente."));
    }

    [HttpPost("{id:int}/pagar")]
    [ProducesResponseType(typeof(ApiResponse<PagoSimuladoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Pagar(int id, CancellationToken cancellationToken)
    {
        var actor = ObtenerActorCliente();
        await ObtenerPedidoPropioAsync(actor, id, cancellationToken);

        var result = await _pedidoService.PagarAsync(id, cancellationToken);

        return Ok(ApiResponse<PagoSimuladoResponse>.Ok(
            result,
            "Pago simulado procesado exitosamente."));
    }

    private ActorContext ObtenerActorCliente()
    {
        var actor = User.ToActorContext();
        _autorizacionService.ExigirClientePropietario(actor, actor.id_cliente ?? 0);
        return actor;
    }

    private async Task<PedidoResponse> ObtenerPedidoPropioAsync(
        ActorContext actor,
        int idPedido,
        CancellationToken cancellationToken)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(idPedido, cancellationToken)
            ?? throw new NotFoundException("El pedido no existe.");

        _autorizacionService.ExigirClientePropietario(actor, pedido.id_cliente);
        return pedido;
    }
}
