using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ProveedorMaterialDataMapper
    {
        public static ProveedorMaterialDataModel ToDataModel(ProveedorMaterialEntity entity) => new()
        {
            id_material = entity.id_material,
            id_proveedor = entity.id_proveedor,
            origen = entity.origen,
            precio_compra = entity.precio_compra,
            cantidad_min = entity.cantidad_min,
            dias_entrega = entity.dias_entrega,
            material_nombre = entity.Material?.mat_nombre,
            codigo_proveedor = entity.Proveedor?.codigo_proveedor,
            proveedor_nombre = entity.Proveedor?.prov_nombre
        };

        public static ProveedorMaterialEntity ToEntity(ProveedorMaterialDataModel model) => new()
        {
            id_material = model.id_material,
            id_proveedor = model.id_proveedor,
            origen = model.origen,
            precio_compra = model.precio_compra,
            cantidad_min = model.cantidad_min,
            dias_entrega = model.dias_entrega
        };

        public static void ApplyToEntity(ProveedorMaterialDataModel model, ProveedorMaterialEntity entity)
        {
            entity.origen = model.origen;
            entity.precio_compra = model.precio_compra;
            entity.cantidad_min = model.cantidad_min;
            entity.dias_entrega = model.dias_entrega;
        }
    }
}
