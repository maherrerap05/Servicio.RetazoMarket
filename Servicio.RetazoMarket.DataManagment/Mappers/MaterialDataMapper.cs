using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class MaterialDataMapper
    {
        public static MaterialDataModel ToDataModel(MaterialEntity entity) => new()
        {
            id_material = entity.id_material,
            id_categoria = entity.id_categoria,
            mat_nombre = entity.mat_nombre,
            unidad_medida = entity.unidad_medida,
            stock_actual = entity.stock_actual,
            mat_estado = entity.mat_estado,
            Categoria = entity.Categoria is null ? null : new CategoriaMaterialResumenDataModel
            {
                id_categoria = entity.Categoria.id_categoria,
                cat_nombre = entity.Categoria.cat_nombre,
                cat_estado = entity.Categoria.cat_estado
            },
            Proveedores = entity.Proveedores.Select(pm => new ProveedorMaterialResumenDataModel
            {
                id_proveedor = pm.id_proveedor,
                prov_nombre = pm.Proveedor?.prov_nombre ?? string.Empty,
                prov_estado = pm.Proveedor?.prov_estado ?? string.Empty,
                origen = pm.origen,
                precio_compra = pm.precio_compra,
                cantidad_min = pm.cantidad_min,
                dias_entrega = pm.dias_entrega
            }).ToList()
        };

        public static MaterialEntity ToEntity(MaterialDataModel model) => new()
        {
            id_material = model.id_material,
            id_categoria = model.id_categoria,
            mat_nombre = model.mat_nombre,
            unidad_medida = model.unidad_medida,
            stock_actual = model.stock_actual,
            mat_estado = model.mat_estado
        };

        public static void ApplyToEntity(MaterialDataModel model, MaterialEntity entity)
        {
            entity.id_categoria = model.id_categoria;
            entity.mat_nombre = model.mat_nombre;
            entity.unidad_medida = model.unidad_medida;
            entity.stock_actual = model.stock_actual;
            entity.mat_estado = model.mat_estado;
        }
    }
}
