using Servicio.RetazoMarket.Business.DTOs.Material;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialDataService _data;
    private readonly ICategoriaMaterialDataService _categorias;

    public MaterialService(IMaterialDataService data, ICategoriaMaterialDataService categorias) =>
        (_data, _categorias) = (data, categorias);

    public async Task<IReadOnlyList<MaterialResponse>> ObtenerTodosAsync(CancellationToken ct = default) =>
        (await _data.ObtenerTodosAsync(ct)).Select(MaterialBusinessMapper.ToResponse).ToList();

    public async Task<MaterialResponse?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var model = await _data.ObtenerPorIdAsync(id, ct);
        return model is null ? null : MaterialBusinessMapper.ToResponse(model);
    }

    public async Task<MaterialResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default)
    {
        var model = await _data.ObtenerPorNombreAsync(nombre.Trim(), ct);
        return model is null ? null : MaterialBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<MaterialResponse>> BuscarAsync(
        MaterialFiltroRequest filtro, CancellationToken ct = default)
    {
        var errores = MaterialValidator.ValidarFiltro(filtro);
        if (errores.Any())
            throw new ValidationException("Filtro inválido.", errores);

        var result = await _data.BuscarAsync(new MaterialFiltroDataModel
        {
            nombre = filtro.nombre?.Trim(),
            id_categoria = filtro.id_categoria,
            estado = filtro.estado?.Trim().ToUpperInvariant(),
            stockMinimo = filtro.stock_minimo,
            stockMaximo = filtro.stock_maximo,
            codigo_proveedor = filtro.codigo_proveedor?.Trim().ToUpperInvariant(),
            PageNumber = filtro.page_number,
            PageSize = filtro.page_size
        }, ct);

        return new DataPagedResult<MaterialResponse>
        {
            Items = result.Items.Select(MaterialBusinessMapper.ToResponse).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<MaterialResponse> CrearAsync(CrearMaterialRequest request, CancellationToken ct = default)
    {
        var errores = MaterialValidator.Validar(request);
        if (errores.Any())
            throw new ValidationException("Solicitud inválida.", errores);

        await ValidarCategoriaActiva(request.id_categoria, ct);
        if (await _data.ExistePorNombreAsync(request.mat_nombre.Trim(), ct))
            throw new ValidationException("Ya existe un material con ese nombre.");

        return MaterialBusinessMapper.ToResponse(
            await _data.CrearAsync(MaterialBusinessMapper.ToDataModel(request), ct));
    }

    public async Task<MaterialResponse?> ActualizarAsync(
        ActualizarMaterialRequest request, CancellationToken ct = default)
    {
        var errores = MaterialValidator.Validar(request);
        if (errores.Any())
            throw new ValidationException("Solicitud inválida.", errores);

        var actual = await _data.ObtenerPorIdAsync(request.id_material, ct);
        if (actual is null) return null;

        await ValidarCategoriaActiva(request.id_categoria, ct);
        var otro = await _data.ObtenerPorNombreAsync(request.mat_nombre.Trim(), ct);
        if (otro is not null && otro.id_material != request.id_material)
            throw new ValidationException("Ya existe otro material con ese nombre.");

        var model = await _data.ActualizarAsync(
            MaterialBusinessMapper.ToDataModel(request, actual.stock_actual), ct);
        return model is null ? null : MaterialBusinessMapper.ToResponse(model);
    }

    public Task<bool> EliminarAsync(int id, CancellationToken ct = default) =>
        _data.EliminarLogicoAsync(id, ct);

    private async Task ValidarCategoriaActiva(int id, CancellationToken ct)
    {
        var categoria = await _categorias.ObtenerPorIdAsync(id, ct);
        if (categoria is null || categoria.cat_estado != "ACT")
            throw new ValidationException("La categoría no existe o está inactiva.");
    }
}
