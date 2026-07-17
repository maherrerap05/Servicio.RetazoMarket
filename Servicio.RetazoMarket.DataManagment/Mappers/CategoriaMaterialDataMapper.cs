using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class CategoriaMaterialDataMapper
    {
        public static CategoriaMaterialDataModel ToDataModel(CategoriaMaterialEntity entity) => new()
        {
            id_categoria = entity.id_categoria,
            cat_nombre = entity.cat_nombre,
            cat_estado = entity.cat_estado
        };

        public static CategoriaMaterialEntity ToEntity(CategoriaMaterialDataModel model) => new()
        {
            id_categoria = model.id_categoria,
            cat_nombre = model.cat_nombre,
            cat_estado = model.cat_estado
        };

        public static void ApplyToEntity(CategoriaMaterialDataModel model, CategoriaMaterialEntity entity)
        {
            entity.cat_nombre = model.cat_nombre;
            entity.cat_estado = model.cat_estado;
        }
    }
}
