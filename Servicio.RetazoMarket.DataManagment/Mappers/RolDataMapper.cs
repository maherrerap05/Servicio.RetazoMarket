using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class RolDataMapper
    {
        public static RolDataModel ToDataModel(RolEntity entity) => new()
        {
            id_rol = entity.id_rol,
            nombre_rol = entity.nombre_rol
        };

        public static RolEntity ToEntity(RolDataModel model) => new()
        {
            id_rol = model.id_rol,
            nombre_rol = model.nombre_rol
        };

        public static void ApplyToEntity(RolDataModel model, RolEntity entity)
        {
            entity.nombre_rol = model.nombre_rol;
        }
    }
}
