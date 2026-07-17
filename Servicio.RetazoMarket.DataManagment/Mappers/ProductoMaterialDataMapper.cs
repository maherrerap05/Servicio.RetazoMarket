using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ProductoMaterialDataMapper
    {
        public static ProductoMaterialDataModel ToDataModel(ProductoMaterialEntity entity) => new()
        {
            id_producto = entity.id_producto,
            id_material = entity.id_material,
            cantidad_req = entity.cantidad_req,
            es_personalizable = entity.es_personalizable,
            material_nombre = entity.Material?.mat_nombre,
            unidad_medida = entity.Material?.unidad_medida
        };

        public static ProductoMaterialEntity ToEntity(ProductoMaterialDataModel model) => new()
        {
            id_producto = model.id_producto,
            id_material = model.id_material,
            cantidad_req = model.cantidad_req,
            es_personalizable = model.es_personalizable
        };

        public static void ApplyToEntity(ProductoMaterialDataModel model, ProductoMaterialEntity entity)
        {
            entity.cantidad_req = model.cantidad_req;
            entity.es_personalizable = model.es_personalizable;
        }
    }
}
