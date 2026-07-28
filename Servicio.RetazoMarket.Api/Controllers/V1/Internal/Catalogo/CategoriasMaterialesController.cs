using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.RetazoMarket.Api.Extensions;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Catalogo;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = AuthorizationExtensions.AdministradorPolicy)]
[Route("api/v{version:apiVersion}/internal/categorias-materiales")]
public class CategoriasMaterialesController : ControllerBase
{
    private readonly ICategoriaMaterialService _service;
    public CategoriasMaterialesController(ICategoriaMaterialService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CategoriaMaterialResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct) => Ok(ApiResponse<IReadOnlyList<CategoriaMaterialResponse>>.Ok(await _service.ObtenerTodosAsync(ct), "Consulta exitosa."));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoriaMaterialResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var result = await _service.ObtenerPorIdAsync(id, ct) ?? throw new NotFoundException("La categoría de material no existe.");
        return Ok(ApiResponse<CategoriaMaterialResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("nombre/{nombre}")]
    [ProducesResponseType(typeof(ApiResponse<CategoriaMaterialResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorNombre(string nombre, CancellationToken ct)
    {
        var result = await _service.ObtenerPorNombreAsync(nombre, ct) ?? throw new NotFoundException("La categoría de material no existe.");
        return Ok(ApiResponse<CategoriaMaterialResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoriaMaterialResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearCategoriaMaterialRequest request, CancellationToken ct)
    {
        var result = await _service.CrearAsync(request, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { version = "1.0", id = result.id_categoria }, ApiResponse<CategoriaMaterialResponse>.Ok(result, "Categoría creada exitosamente."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoriaMaterialResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaMaterialRequest request, CancellationToken ct)
    {
        request.id_categoria = id;
        var result = await _service.ActualizarAsync(request, ct) ?? throw new NotFoundException("La categoría de material no existe.");
        return Ok(ApiResponse<CategoriaMaterialResponse>.Ok(result, "Categoría actualizada exitosamente."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        if (!await _service.EliminarAsync(id, ct)) throw new NotFoundException("La categoría de material no existe.");
        return Ok(ApiResponse<bool>.Ok(true, "Categoría eliminada lógicamente."));
    }
}
