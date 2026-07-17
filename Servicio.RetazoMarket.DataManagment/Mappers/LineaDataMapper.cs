using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class LineaDataMapper
    {
        public static LineaDataModel ToDataModel(LineaEntity entity) => new()
        {
            id_linea = entity.id_linea,
            lin_nombre = entity.lin_nombre,
            lin_estado = entity.lin_estado
        };

        public static LineaEntity ToEntity(LineaDataModel model) => new()
        {
            id_linea = model.id_linea,
            lin_nombre = model.lin_nombre,
            lin_estado = model.lin_estado
        };

        public static void ApplyToEntity(LineaDataModel model, LineaEntity entity)
        {
            entity.lin_nombre = model.lin_nombre;
            entity.lin_estado = model.lin_estado;
        }
    }
}
