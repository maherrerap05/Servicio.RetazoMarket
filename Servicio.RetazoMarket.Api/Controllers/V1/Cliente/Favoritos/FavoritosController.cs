using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.DTOs.Favorito;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Cliente.Favoritos;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.ClientePolicy)]
[Route("api/v{version:apiVersion}/cliente/favoritos")]
public class FavoritosController : ControllerBase
{
    private readonly IFavoritoService _favoritoService;
    private readonly IAutorizacionService _autorizacionService;

    public FavoritosController(
        IFavoritoService favoritoService,
        IAutorizacionService autorizacionService)
    {
        _favoritoService = favoritoService;
        _autorizacionService = autorizacionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FavoritoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        var result = await _favoritoService.ObtenerPorClienteAsync(
            idCliente,
            idCliente,
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyList<FavoritoResponse>>.Ok(
            result,
            "Consulta de favoritos exitosa."));
    }

    [HttpGet("buscar")]
    [ProducesResponseType(
        typeof(ApiResponse<DataPagedResult<FavoritoResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Buscar(
        [FromQuery] FavoritoFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        filtro.id_cliente = idCliente;

        var result = await _favoritoService.BuscarAsync(filtro, idCliente, cancellationToken);

        return Ok(ApiResponse<DataPagedResult<FavoritoResponse>>.Ok(
            result,
            "Consulta paginada de favoritos exitosa."));
    }

    [HttpGet("{idProducto:int}")]
    [ProducesResponseType(typeof(ApiResponse<FavoritoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorProducto(
        int idProducto,
        CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        var result = await _favoritoService.ObtenerAsync(
            idCliente,
            idProducto,
            idCliente,
            cancellationToken)
            ?? throw new NotFoundException("El producto no se encuentra en los favoritos del cliente.");

        return Ok(ApiResponse<FavoritoResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FavoritoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Agregar(
        [FromBody] AgregarFavoritoRequest request,
        CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        request.id_cliente = idCliente;

        var result = await _favoritoService.AgregarAsync(request, idCliente, cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorProducto),
            new { version = "1.0", idProducto = result.id_producto },
            ApiResponse<FavoritoResponse>.Ok(result, "Producto agregado a favoritos."));
    }

    [HttpDelete("{idProducto:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Quitar(
        int idProducto,
        CancellationToken cancellationToken)
    {
        var idCliente = ObtenerActorCliente().id_cliente!.Value;
        var eliminado = await _favoritoService.QuitarAsync(
            idCliente,
            idProducto,
            idCliente,
            cancellationToken);

        if (!eliminado)
            throw new NotFoundException("El producto no se encuentra en los favoritos del cliente.");

        return Ok(ApiResponse<bool>.Ok(true, "Producto eliminado de favoritos."));
    }

    private ActorContext ObtenerActorCliente()
    {
        var actor = User.ToActorContext();
        _autorizacionService.ExigirClientePropietario(actor, actor.id_cliente ?? 0);
        return actor;
    }
}
