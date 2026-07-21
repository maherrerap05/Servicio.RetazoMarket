using Servicio.RetazoMarket.Business.DTOs.Proveedor;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorDataService _data;

    public ProveedorService(IProveedorDataService data) => _data = data;

    public async Task<IReadOnlyList<ProveedorResponse>> ObtenerTodosAsync(CancellationToken ct = default) =>
        (await _data.ObtenerTodosAsync(ct)).Select(ProveedorBusinessMapper.ToResponse).ToList();

    public async Task<ProveedorResponse?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var model = await _data.ObtenerPorIdAsync(id, ct);
        return model is null ? null : ProveedorBusinessMapper.ToResponse(model);
    }

    public async Task<ProveedorResponse?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(codigo) || codigo.Trim().Length > 200)
            throw new ValidationException("El código del proveedor es inválido.");

        var model = await _data.ObtenerPorCodigoAsync(NormalizarCodigo(codigo), ct);
        return model is null ? null : ProveedorBusinessMapper.ToResponse(model);
    }

    public async Task<ProveedorResponse?> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default)
    {
        var model = await _data.ObtenerPorCorreoAsync(NormalizarCorreo(correo), ct);
        return model is null ? null : ProveedorBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<ProveedorResponse>> BuscarAsync(
        ProveedorFiltroRequest filtro, CancellationToken ct = default)
    {
        var errores = ProveedorValidator.ValidarFiltro(filtro);
        if (errores.Any())
            throw new ValidationException("Filtro inválido.", errores);

        var result = await _data.BuscarAsync(new ProveedorFiltroDataModel
        {
            codigo_proveedor = filtro.codigo_proveedor is null ? null : NormalizarCodigo(filtro.codigo_proveedor),
            nombre = filtro.nombre?.Trim(),
            correo = filtro.correo is null ? null : NormalizarCorreo(filtro.correo),
            estado = filtro.estado?.Trim().ToUpperInvariant(),
            id_material = filtro.id_material,
            PageNumber = filtro.page_number,
            PageSize = filtro.page_size
        }, ct);

        return new DataPagedResult<ProveedorResponse>
        {
            Items = result.Items.Select(ProveedorBusinessMapper.ToResponse).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<ProveedorResponse> CrearAsync(CrearProveedorRequest request, CancellationToken ct = default)
    {
        var errores = ProveedorValidator.Validar(request);
        if (errores.Any())
            throw new ValidationException("Solicitud inválida.", errores);

        var codigo = NormalizarCodigo(request.codigo_proveedor);
        if (await _data.ExistePorCodigoAsync(codigo, ct))
            throw new ValidationException("Ya existe un proveedor con ese código.");
        if (await _data.ExistePorCorreoAsync(NormalizarCorreo(request.prov_correo), ct))
            throw new ValidationException("Ya existe un proveedor con ese correo.");

        return ProveedorBusinessMapper.ToResponse(
            await _data.CrearAsync(ProveedorBusinessMapper.ToDataModel(request), ct));
    }

    public async Task<ProveedorResponse?> ActualizarAsync(
        ActualizarProveedorRequest request, CancellationToken ct = default)
    {
        var errores = ProveedorValidator.Validar(request);
        if (errores.Any())
            throw new ValidationException("Solicitud inválida.", errores);

        var proveedorPorCodigo = await _data.ObtenerPorCodigoAsync(NormalizarCodigo(request.codigo_proveedor), ct);
        if (proveedorPorCodigo is not null && proveedorPorCodigo.id_proveedor != request.id_proveedor)
            throw new ValidationException("Ya existe otro proveedor con ese código.");

        var proveedorPorCorreo = await _data.ObtenerPorCorreoAsync(NormalizarCorreo(request.prov_correo), ct);
        if (proveedorPorCorreo is not null && proveedorPorCorreo.id_proveedor != request.id_proveedor)
            throw new ValidationException("Ya existe otro proveedor con ese correo.");

        var model = await _data.ActualizarAsync(ProveedorBusinessMapper.ToDataModel(request), ct);
        return model is null ? null : ProveedorBusinessMapper.ToResponse(model);
    }

    public Task<bool> EliminarAsync(int id, CancellationToken ct = default) =>
        _data.EliminarLogicoAsync(id, ct);

    private static string NormalizarCodigo(string codigo) => codigo.Trim().ToUpperInvariant();
    private static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
}
