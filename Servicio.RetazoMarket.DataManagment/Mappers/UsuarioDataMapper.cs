using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class UsuarioDataMapper
    {
        public static UsuarioDataModel ToDataModel(UsuarioEntity entity) => new()
        {
            id_usuario = entity.id_usuario,
            id_cliente = entity.id_cliente,
            id_rol = entity.id_rol,
            nombre = entity.nombre,
            correo = entity.correo,
            contrasena_hash = entity.contrasena_hash,
            usr_estado = entity.usr_estado,
            ultimo_acceso = entity.ultimo_acceso,
            Rol = entity.Rol is null ? null : new RolResumenDataModel
            {
                id_rol = entity.Rol.id_rol,
                nombre_rol = entity.Rol.nombre_rol
            },
            Cliente = entity.Cliente is null ? null : new ClienteResumenDataModel
            {
                id_cliente = entity.Cliente.id_cliente,
                nombre = entity.Cliente.nombre,
                apellidos = entity.Cliente.apellidos,
                correo = entity.Cliente.correo
            }
        };

        public static UsuarioEntity ToEntity(UsuarioDataModel model) => new()
        {
            id_usuario = model.id_usuario,
            id_cliente = model.id_cliente,
            id_rol = model.id_rol,
            nombre = model.nombre,
            correo = model.correo,
            contrasena_hash = model.contrasena_hash,
            usr_estado = model.usr_estado,
            ultimo_acceso = model.ultimo_acceso
        };

        public static void ApplyToEntity(UsuarioDataModel model, UsuarioEntity entity)
        {
            entity.id_cliente = model.id_cliente;
            entity.id_rol = model.id_rol;
            entity.nombre = model.nombre;
            entity.correo = model.correo;
            entity.contrasena_hash = model.contrasena_hash;
            entity.usr_estado = model.usr_estado;
            entity.ultimo_acceso = model.ultimo_acceso;
        }
    }
}
