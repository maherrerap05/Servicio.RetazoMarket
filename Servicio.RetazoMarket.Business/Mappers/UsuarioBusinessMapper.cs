using Servicio.RetazoMarket.Business.DTOs.Usuario;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers;
public static class UsuarioBusinessMapper
{
    public static UsuarioDataModel ToDataModel(CrearUsuarioRequest r, string hash) => new() { id_cliente=r.id_cliente, id_rol=r.id_rol, nombre=r.nombre.Trim(), correo=r.correo.Trim().ToLowerInvariant(), contrasena_hash=hash, usr_estado=r.usr_estado.Trim().ToUpperInvariant(), ultimo_acceso=DateTime.UtcNow };
    public static UsuarioDataModel ToDataModel(ActualizarUsuarioRequest r, string hash, DateTime ultimoAcceso) => new() { id_usuario=r.id_usuario, id_cliente=r.id_cliente, id_rol=r.id_rol, nombre=r.nombre.Trim(), correo=r.correo.Trim().ToLowerInvariant(), contrasena_hash=hash, usr_estado=r.usr_estado.Trim().ToUpperInvariant(), ultimo_acceso=ultimoAcceso };
    public static UsuarioResponse ToResponse(UsuarioDataModel m) => new() { id_usuario=m.id_usuario, id_cliente=m.id_cliente, id_rol=m.id_rol, nombre=m.nombre, correo=m.correo, usr_estado=m.usr_estado, ultimo_acceso=m.ultimo_acceso, nombre_rol=m.Rol?.nombre_rol };
}
