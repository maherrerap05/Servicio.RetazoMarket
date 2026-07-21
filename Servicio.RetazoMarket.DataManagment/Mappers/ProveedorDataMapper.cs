using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ProveedorDataMapper
    {
        public static ProveedorDataModel ToDataModel(ProveedorEntity entity) => new()
        {
            id_proveedor = entity.id_proveedor,
            codigo_proveedor = entity.codigo_proveedor,
            prov_nombre = entity.prov_nombre,
            prov_telefono = entity.prov_telefono,
            prov_correo = entity.prov_correo,
            prov_direccion = entity.prov_direccion,
            prov_estado = entity.prov_estado,
            Materiales = entity.MaterialesSuministrados.Select(pm => new MaterialProveedorResumenDataModel
            {
                id_material = pm.id_material,
                mat_nombre = pm.Material?.mat_nombre ?? string.Empty,
                unidad_medida = pm.Material?.unidad_medida ?? string.Empty,
                origen = pm.origen,
                precio_compra = pm.precio_compra,
                cantidad_min = pm.cantidad_min,
                dias_entrega = pm.dias_entrega
            }).ToList()
        };

        public static ProveedorEntity ToEntity(ProveedorDataModel model) => new()
        {
            id_proveedor = model.id_proveedor,
            codigo_proveedor = model.codigo_proveedor,
            prov_nombre = model.prov_nombre,
            prov_telefono = model.prov_telefono,
            prov_correo = model.prov_correo,
            prov_direccion = model.prov_direccion,
            prov_estado = model.prov_estado
        };

        public static void ApplyToEntity(ProveedorDataModel model, ProveedorEntity entity)
        {
            entity.codigo_proveedor = model.codigo_proveedor;
            entity.prov_nombre = model.prov_nombre;
            entity.prov_telefono = model.prov_telefono;
            entity.prov_correo = model.prov_correo;
            entity.prov_direccion = model.prov_direccion;
            entity.prov_estado = model.prov_estado;
        }
    }
}
