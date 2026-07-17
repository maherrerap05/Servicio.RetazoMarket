using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ClienteDataMapper
    {
        public static ClienteDataModel ToDataModel(ClienteEntity entity) => new()
        {
            id_cliente = entity.id_cliente,
            nombre = entity.nombre,
            apellidos = entity.apellidos,
            correo = entity.correo,
            telefono = entity.telefono,
            direccion = entity.direccion,
            origen = entity.origen,
            cli_estado = entity.cli_estado,
            fecha_registro = entity.fecha_registro
        };

        public static ClienteEntity ToEntity(ClienteDataModel model) => new()
        {
            id_cliente = model.id_cliente,
            nombre = model.nombre,
            apellidos = model.apellidos,
            correo = model.correo,
            telefono = model.telefono,
            direccion = model.direccion,
            origen = model.origen,
            cli_estado = model.cli_estado,
            fecha_registro = model.fecha_registro
        };

        public static void ApplyToEntity(ClienteDataModel model, ClienteEntity entity)
        {
            entity.nombre = model.nombre;
            entity.apellidos = model.apellidos;
            entity.correo = model.correo;
            entity.telefono = model.telefono;
            entity.direccion = model.direccion;
            entity.origen = model.origen;
            entity.cli_estado = model.cli_estado;
        }
    }
}
