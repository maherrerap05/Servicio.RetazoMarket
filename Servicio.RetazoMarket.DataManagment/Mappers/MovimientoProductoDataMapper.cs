using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class MovimientoProductoDataMapper
    {
        public static MovimientoProductoDataModel ToDataModel(MovimientoProductoEntity entity) => new()
        {
            id_movimiento2 = entity.id_movimiento2,
            id_producto = entity.id_producto,
            tipo_movimiento = entity.tipo_movimiento,
            cantidad = entity.cantidad,
            fecha_mov = entity.fecha_mov,
            motivo_mov = entity.motivo_mov,
            Producto = entity.Producto is null ? null : new ProductoMovimientoResumenDataModel
            {
                id_producto = entity.Producto.id_producto,
                prod_nombre = entity.Producto.prod_nombre,
                stock_actual = entity.Producto.stock_actual,
                prod_estado = entity.Producto.prod_estado
            }
        };

        public static MovimientoProductoEntity ToEntity(MovimientoProductoDataModel model) => new()
        {
            id_movimiento2 = model.id_movimiento2,
            id_producto = model.id_producto,
            tipo_movimiento = model.tipo_movimiento,
            cantidad = model.cantidad,
            fecha_mov = model.fecha_mov,
            motivo_mov = model.motivo_mov
        };
    }
}
