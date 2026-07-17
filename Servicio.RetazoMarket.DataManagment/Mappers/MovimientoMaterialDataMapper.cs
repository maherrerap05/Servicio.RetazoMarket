using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class MovimientoMaterialDataMapper
    {
        public static MovimientoMaterialDataModel ToDataModel(MovimientoMaterialEntity entity) => new()
        {
            id_movimiento = entity.id_movimiento,
            id_material = entity.id_material,
            tipo_movimiento = entity.tipo_movimiento,
            cantidad = entity.cantidad,
            fecha_mov = entity.fecha_mov,
            motivo_mov = entity.motivo_mov,
            metodo_pago = entity.metodo_pago,
            Material = entity.Material is null ? null : new MaterialMovimientoResumenDataModel
            {
                id_material = entity.Material.id_material,
                mat_nombre = entity.Material.mat_nombre,
                unidad_medida = entity.Material.unidad_medida,
                stock_actual = entity.Material.stock_actual
            }
        };

        public static MovimientoMaterialEntity ToEntity(MovimientoMaterialDataModel model) => new()
        {
            id_movimiento = model.id_movimiento,
            id_material = model.id_material,
            tipo_movimiento = model.tipo_movimiento,
            cantidad = model.cantidad,
            fecha_mov = model.fecha_mov,
            motivo_mov = model.motivo_mov,
            metodo_pago = model.metodo_pago
        };
    }
}
