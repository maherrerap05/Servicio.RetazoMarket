using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class DescuentoDataMapper
    {
        public static DescuentoDataModel ToDataModel(DescuentoEntity entity)
        {
            return new DescuentoDataModel
            {
                id_descuento = entity.id_descuento,
                id_producto = entity.id_producto,
                cantidad_minima = entity.cantidad_minima,
                porcentaje = entity.porcentaje,
                estado = entity.estado,
                Producto = entity.Producto is null
                    ? null
                    : new ProductoDescuentoResumenDataModel
                    {
                        id_producto = entity.Producto.id_producto,
                        prod_nombre = entity.Producto.prod_nombre,
                        prod_estado = entity.Producto.prod_estado,
                        tiene_descuentos = entity.Producto.tiene_descuentos
                    }
            };
        }

        public static DescuentoEntity ToEntity(DescuentoDataModel model)
        {
            return new DescuentoEntity
            {
                id_descuento = model.id_descuento,
                id_producto = model.id_producto,
                cantidad_minima = model.cantidad_minima,
                porcentaje = model.porcentaje,
                estado = model.estado
            };
        }

        public static void ApplyToEntity(DescuentoDataModel model, DescuentoEntity entity)
        {
            entity.id_producto = model.id_producto;
            entity.cantidad_minima = model.cantidad_minima;
            entity.porcentaje = model.porcentaje;
            entity.estado = model.estado;
        }
    }
}
