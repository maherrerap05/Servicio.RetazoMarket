using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ProductoDataMapper
    {
        public static ProductoDataModel ToDataModel(ProductoEntity entity) => new()
        {
            id_producto = entity.id_producto,
            id_linea = entity.id_linea,
            prod_nombre = entity.prod_nombre,
            prod_peso = entity.prod_peso,
            prod_descripcion = entity.prod_descripcion,
            colores = JsonDataMapper.Clone(entity.colores),
            costo_mat_prim = entity.costo_mat_prim,
            costo_mano_obra = entity.costo_mano_obra,
            porcentaje_margen_ganancia = entity.porcentaje_margen_ganancia,
            porcentaje_gastos_fijos = entity.porcentaje_gastos_fijos,
            porcentaje_gastos_operativos = entity.porcentaje_gastos_operativos,
            precio_base = entity.precio_base,
            es_personalizable = entity.es_personalizable,
            stock_actual = entity.stock_actual,
            tiene_descuentos = entity.tiene_descuentos,
            prod_estado = entity.prod_estado,
            Linea = entity.Linea is null ? null : new LineaProductoResumenDataModel
            {
                id_linea = entity.Linea.id_linea,
                lin_nombre = entity.Linea.lin_nombre,
                lin_estado = entity.Linea.lin_estado
            },
            Materiales = entity.Materiales.Select(pm => new ProductoMaterialResumenDataModel
            {
                id_material = pm.id_material,
                mat_nombre = pm.Material?.mat_nombre ?? string.Empty,
                unidad_medida = pm.Material?.unidad_medida ?? string.Empty,
                cantidad_req = pm.cantidad_req,
                es_personalizable = pm.es_personalizable
            }).ToList(),
            Personalizaciones = entity.Personalizaciones.Select(p => new PersonalizacionResumenDataModel
            {
                id_opcion = p.id_opcion,
                nombre_atr = p.nombre_atr,
                tipo_valor = p.tipo_valor.Trim(),
                valores_json = JsonDataMapper.Clone(p.valores_json),
                costo_adicional = p.costo_adicional
            }).ToList(),
            Imagenes = entity.Imagenes.OrderBy(i => i.orden).ThenBy(i => i.id_imagen).Select(i => new ImagenResumenDataModel
            {
                id_imagen = i.id_imagen,
                url = i.url,
                es_principal = i.es_principal,
                orden = i.orden
            }).ToList(),
            Descuentos = entity.Descuentos.OrderBy(d => d.cantidad_minima).ThenBy(d => d.id_descuento).Select(d => new DescuentoResumenDataModel
            {
                id_descuento = d.id_descuento,
                cantidad_minima = d.cantidad_minima,
                porcentaje = d.porcentaje,
                estado = d.estado
            }).ToList()
        };

        public static ProductoEntity ToEntity(ProductoDataModel model) => new()
        {
            id_producto = model.id_producto,
            id_linea = model.id_linea,
            prod_nombre = model.prod_nombre,
            prod_peso = model.prod_peso,
            prod_descripcion = model.prod_descripcion,
            colores = JsonDataMapper.ToDocument(model.colores),
            costo_mat_prim = model.costo_mat_prim,
            costo_mano_obra = model.costo_mano_obra,
            porcentaje_margen_ganancia = model.porcentaje_margen_ganancia,
            porcentaje_gastos_fijos = model.porcentaje_gastos_fijos,
            porcentaje_gastos_operativos = model.porcentaje_gastos_operativos,
            precio_base = model.precio_base,
            es_personalizable = model.es_personalizable,
            stock_actual = model.stock_actual,
            tiene_descuentos = model.tiene_descuentos,
            prod_estado = model.prod_estado
        };

        public static void ApplyToEntity(ProductoDataModel model, ProductoEntity entity)
        {
            entity.id_linea = model.id_linea;
            entity.prod_nombre = model.prod_nombre;
            entity.prod_peso = model.prod_peso;
            entity.prod_descripcion = model.prod_descripcion;
            entity.colores = JsonDataMapper.ToDocument(model.colores);
            entity.costo_mat_prim = model.costo_mat_prim;
            entity.costo_mano_obra = model.costo_mano_obra;
            entity.porcentaje_margen_ganancia = model.porcentaje_margen_ganancia;
            entity.porcentaje_gastos_fijos = model.porcentaje_gastos_fijos;
            entity.porcentaje_gastos_operativos = model.porcentaje_gastos_operativos;
            entity.precio_base = model.precio_base;
            entity.es_personalizable = model.es_personalizable;
            entity.stock_actual = model.stock_actual;
            entity.tiene_descuentos = model.tiene_descuentos;
            entity.prod_estado = model.prod_estado;
        }
    }
}
