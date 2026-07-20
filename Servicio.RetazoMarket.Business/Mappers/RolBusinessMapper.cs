using Servicio.RetazoMarket.Business.DTOs.Rol;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers
{
    public static class RolBusinessMapper
    {
        public static RolDataModel ToDataModel(CrearRolRequest request) => new()
        {
            nombre_rol = request.nombre_rol.Trim().ToUpperInvariant()
        };

        public static RolDataModel ToDataModel(ActualizarRolRequest request) => new()
        {
            id_rol = request.id_rol,
            nombre_rol = request.nombre_rol.Trim().ToUpperInvariant()
        };

        public static RolResponse ToResponse(RolDataModel model) => new()
        {
            id_rol = model.id_rol,
            nombre_rol = model.nombre_rol
        };
    }
}
