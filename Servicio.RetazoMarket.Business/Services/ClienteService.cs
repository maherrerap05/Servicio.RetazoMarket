using Servicio.RetazoMarket.Business.DTOs.Cliente;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services;
public class ClienteService : IClienteService
{
    private readonly IClienteDataService _clientes;
    private readonly IUsuarioDataService _usuarios;
    public ClienteService(IClienteDataService clientes, IUsuarioDataService usuarios) => (_clientes, _usuarios) = (clientes, usuarios);

    public async Task<IReadOnlyList<ClienteResponse>> ObtenerTodosAsync(CancellationToken ct=default) => (await _clientes.ObtenerTodosAsync(ct)).Select(ClienteBusinessMapper.ToResponse).ToList();
    public async Task<ClienteResponse?> ObtenerPorIdAsync(int id, CancellationToken ct=default) { var m=await _clientes.ObtenerPorIdAsync(id,ct); return m is null?null:ClienteBusinessMapper.ToResponse(m); }
    public async Task<ClienteResponse?> ObtenerPorCorreoAsync(string correo, CancellationToken ct=default) { var m=await _clientes.ObtenerPorCorreoAsync(NormalizarCorreo(correo),ct); return m is null?null:ClienteBusinessMapper.ToResponse(m); }

    public async Task<DataPagedResult<ClienteResponse>> BuscarAsync(ClienteFiltroRequest f, CancellationToken ct=default)
    {
        var errors=ClienteValidator.ValidarFiltro(f); if(errors.Any()) throw new ValidationException("Filtro inválido.",errors);
        var r=await _clientes.BuscarAsync(new ClienteFiltroDataModel { nombre=f.nombre?.Trim(), apellidos=f.apellidos?.Trim(), correo=f.correo is null?null:NormalizarCorreo(f.correo), telefono=f.telefono?.Trim(), origen=f.origen?.Trim().ToUpperInvariant(), estado=f.estado?.Trim().ToUpperInvariant(), PageNumber=f.page_number, PageSize=f.page_size },ct);
        return new() { Items=r.Items.Select(ClienteBusinessMapper.ToResponse).ToList(), PageNumber=r.PageNumber, PageSize=r.PageSize, TotalRecords=r.TotalRecords };
    }

    public async Task<ClienteResponse> CrearAsync(CrearClienteRequest request, CancellationToken ct=default)
    {
        var errors=ClienteValidator.ValidarCreacion(request); if(errors.Any()) throw new ValidationException("Solicitud inválida.",errors);
        var correo=NormalizarCorreo(request.correo);
        if(await _clientes.ExistePorCorreoAsync(correo,ct) || await _usuarios.ExistePorCorreoAsync(correo,ct)) throw new ValidationException("El correo ya pertenece a otra persona.");
        return ClienteBusinessMapper.ToResponse(await _clientes.CrearAsync(ClienteBusinessMapper.ToDataModel(request),ct));
    }

    public async Task<ClienteResponse?> ActualizarAsync(ActualizarClienteRequest request, CancellationToken ct=default)
    {
        var errors=ClienteValidator.ValidarActualizacion(request); if(errors.Any()) throw new ValidationException("Solicitud inválida.",errors);
        var actual=await _clientes.ObtenerPorIdAsync(request.id_cliente,ct); if(actual is null) return null;
        var correo=NormalizarCorreo(request.correo);
        var otroCliente=await _clientes.ObtenerPorCorreoAsync(correo,ct); if(otroCliente is not null && otroCliente.id_cliente!=request.id_cliente) throw new ValidationException("El correo ya pertenece a otro cliente.");
        var usuario=await _usuarios.ObtenerPorCorreoAsync(correo,ct); if(usuario is not null && usuario.id_cliente!=request.id_cliente) throw new ValidationException("El correo ya pertenece a otra persona.");
        var updated=await _clientes.ActualizarAsync(ClienteBusinessMapper.ToDataModel(request),ct); return updated is null?null:ClienteBusinessMapper.ToResponse(updated);
    }

    public Task<bool> EliminarAsync(int id,CancellationToken ct=default)=>_clientes.EliminarLogicoAsync(id,ct);
    private static string NormalizarCorreo(string correo)=>correo.Trim().ToLowerInvariant();
}
