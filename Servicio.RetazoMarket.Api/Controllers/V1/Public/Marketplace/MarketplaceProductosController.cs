using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.Producto;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Public.Marketplace;

[ApiController]
[ApiVersion("1.0")]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/marketplace/productos")]
public class MarketplaceProductosController : ControllerBase
{
    private readonly IProductoMarketplaceService _productoService;

    public MarketplaceProductosController(IProductoMarketplaceService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<DataPagedResult<ProductoMarketplaceResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buscar(
        [FromQuery] ProductoMarketplaceFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _productoService.BuscarAsync(filtro, cancellationToken);

        return Ok(ApiResponse<DataPagedResult<ProductoMarketplaceResponse>>.Ok(
            result,
            "Consulta de productos exitosa."));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProductoMarketplaceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _productoService.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El producto no existe o no se encuentra disponible.");

        return Ok(ApiResponse<ProductoMarketplaceResponse>.Ok(result, "Consulta exitosa."));
    }
}
