using Servicio.RetazoMarket.Business.DTOs.Usuario;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.Business.Services;
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioDataService _usuarios;
    private readonly IClienteDataService _clientes;
    private readonly IRolDataService _roles;
    private readonly IPasswordHashService _passwords;
    public UsuarioService(IUsuarioDataService usuarios,IClienteDataService clientes,IRolDataService roles,IPasswordHashService passwords)=>(_usuarios,_clientes,_roles,_passwords)=(usuarios,clientes,roles,passwords);

    public async Task<IReadOnlyList<UsuarioResponse>> ObtenerTodosAsync(CancellationToken ct=default)=>(await _usuarios.ObtenerTodosAsync(ct)).Select(UsuarioBusinessMapper.ToResponse).ToList();
    public async Task<UsuarioResponse?> ObtenerPorIdAsync(int id,CancellationToken ct=default){var m=await _usuarios.ObtenerPorIdAsync(id,ct);return m is null?null:UsuarioBusinessMapper.ToResponse(m);}
    public async Task<UsuarioResponse?> ObtenerPorCorreoAsync(string correo,CancellationToken ct=default){var m=await _usuarios.ObtenerPorCorreoAsync(NormalizarCorreo(correo),ct);return m is null?null:UsuarioBusinessMapper.ToResponse(m);}
    public async Task<DataPagedResult<UsuarioResponse>> BuscarAsync(UsuarioFiltroRequest f,CancellationToken ct=default)
    {
        var errors=UsuarioValidator.ValidarFiltro(f);if(errors.Any())throw new ValidationException("Filtro inválido.",errors);
        var r=await _usuarios.BuscarAsync(new UsuarioFiltroDataModel{nombre=f.nombre?.Trim(),correo=f.correo is null?null:NormalizarCorreo(f.correo),estado=f.estado?.Trim().ToUpperInvariant(),id_rol=f.id_rol,id_cliente=f.id_cliente,PageNumber=f.page_number,PageSize=f.page_size},ct);
        return new(){Items=r.Items.Select(UsuarioBusinessMapper.ToResponse).ToList(),PageNumber=r.PageNumber,PageSize=r.PageSize,TotalRecords=r.TotalRecords};
    }
    public async Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request,CancellationToken ct=default)
    {
        var errors=UsuarioValidator.ValidarCreacion(request);if(errors.Any())throw new ValidationException("Solicitud inválida.",errors);
        await ValidarRelacionesYCorreo(request.id_rol,request.id_cliente,NormalizarCorreo(request.correo),null,ct);
        var created=await _usuarios.CrearAsync(UsuarioBusinessMapper.ToDataModel(request,_passwords.Hash(request.contrasena)),ct);return UsuarioBusinessMapper.ToResponse(created);
    }
    public async Task<UsuarioResponse?> ActualizarAsync(ActualizarUsuarioRequest request,CancellationToken ct=default)
    {
        var errors=UsuarioValidator.ValidarActualizacion(request);if(errors.Any())throw new ValidationException("Solicitud inválida.",errors);
        var actual=await _usuarios.ObtenerPorIdAsync(request.id_usuario,ct);if(actual is null)return null;
        await ValidarRelacionesYCorreo(request.id_rol,request.id_cliente,NormalizarCorreo(request.correo),request.id_usuario,ct);
        var updated=await _usuarios.ActualizarAsync(UsuarioBusinessMapper.ToDataModel(request,actual.contrasena_hash,actual.ultimo_acceso),ct);return updated is null?null:UsuarioBusinessMapper.ToResponse(updated);
    }
    public async Task<bool> CambiarContrasenaAsync(CambiarContrasenaRequest request,CancellationToken ct=default)
    {
        if(request is null||request.id_usuario<=0||string.IsNullOrWhiteSpace(request.contrasena_actual)||string.IsNullOrEmpty(request.contrasena_nueva)||request.contrasena_nueva.Length<8||request.contrasena_nueva.Length>128)throw new ValidationException("Los datos para cambiar la contraseña no son válidos.");
        var actual=await _usuarios.ObtenerPorIdAsync(request.id_usuario,ct);if(actual is null)return false;
        if(!_passwords.Verify(request.contrasena_actual,actual.contrasena_hash))throw new UnauthorizedBusinessException("La contraseña actual es incorrecta.");
        actual.contrasena_hash=_passwords.Hash(request.contrasena_nueva);await _usuarios.ActualizarAsync(actual,ct);return true;
    }
    public Task<bool> EliminarAsync(int id,CancellationToken ct=default)=>_usuarios.EliminarLogicoAsync(id,ct);

    private async Task ValidarRelacionesYCorreo(int idRol,int? idCliente,string correo,int? idActual,CancellationToken ct)
    {
        var rol=await _roles.ObtenerPorIdAsync(idRol,ct);if(rol is null)throw new ValidationException("El rol indicado no existe.");
        var esCliente=rol.nombre_rol=="CLIENTE";
        if(esCliente&&!idCliente.HasValue)throw new ValidationException("Un usuario con rol CLIENTE debe estar asociado a un cliente.");
        if(!esCliente&&idCliente.HasValue)throw new ValidationException("Los usuarios internos no deben estar asociados a un cliente.");
        var otro=await _usuarios.ObtenerPorCorreoAsync(correo,ct);if(otro is not null&&otro.id_usuario!=idActual)throw new ValidationException("El correo ya pertenece a otro usuario.");
        var clienteCorreo=await _clientes.ObtenerPorCorreoAsync(correo,ct);
        if(idCliente.HasValue){var cliente=await _clientes.ObtenerPorIdAsync(idCliente.Value,ct);if(cliente is null||cliente.cli_estado!="ACT")throw new ValidationException("El cliente asociado no existe o está inactivo.");if(!string.Equals(cliente.correo,correo,StringComparison.OrdinalIgnoreCase))throw new ValidationException("El correo del usuario debe coincidir con el de su cliente asociado.");}
        else if(clienteCorreo is not null)throw new ValidationException("El correo ya pertenece a un cliente.");
    }
    private static string NormalizarCorreo(string correo)=>correo.Trim().ToLowerInvariant();
}
