using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ProductoPedidoDataMapper
    {
        public static ProductoPedidoDataModel ToDataModel(ProductoPedidoEntity entity) => new()
        {
            id_producto = entity.id_producto,
            id_pedido = entity.id_pedido,
            cantidad = entity.cantidad,
            precio_unitario = entity.precio_unitario,
            porcentaje_descuento = entity.porcentaje_descuento,
            monto_descuento = entity.monto_descuento,
            subtotal_item = entity.subtotal_item,
            personalizacion_selec = JsonDataMapper.Clone(entity.personalizacion_selec),
            Producto = entity.Producto is null ? null : new ProductoPedidoResumenDataModel
            {
                id_producto = entity.Producto.id_producto,
                prod_nombre = entity.Producto.prod_nombre,
                prod_descripcion = entity.Producto.prod_descripcion
            }
        };

        public static ProductoPedidoEntity ToEntity(ProductoPedidoDataModel model) => new()
        {
            id_producto = model.id_producto,
            id_pedido = model.id_pedido,
            cantidad = model.cantidad,
            precio_unitario = model.precio_unitario,
            porcentaje_descuento = model.porcentaje_descuento,
            monto_descuento = model.monto_descuento,
            subtotal_item = model.subtotal_item,
            personalizacion_selec = JsonDataMapper.ToDocument(model.personalizacion_selec)
        };

        public static void ApplyToEntity(ProductoPedidoDataModel model, ProductoPedidoEntity entity)
        {
            entity.cantidad = model.cantidad;
            entity.precio_unitario = model.precio_unitario;
            entity.porcentaje_descuento = model.porcentaje_descuento;
            entity.monto_descuento = model.monto_descuento;
            entity.subtotal_item = model.subtotal_item;
            entity.personalizacion_selec = JsonDataMapper.ToDocument(model.personalizacion_selec);
        }
    }
}
