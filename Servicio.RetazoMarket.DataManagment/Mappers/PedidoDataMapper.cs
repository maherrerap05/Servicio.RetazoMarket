using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class PedidoDataMapper
    {
        public static PedidoDataModel ToDataModel(PedidoEntity entity) => new()
        {
            id_pedido = entity.id_pedido,
            id_metodo = entity.id_metodo,
            id_cliente = entity.id_cliente,
            fecha_hora = entity.fecha_hora,
            estado = entity.estado,
            entrega_fisica = entity.entrega_fisica,
            subtotal = entity.subtotal,
            iva = entity.iva,
            total = entity.total,
            fecha_pago = entity.fecha_pago,
            Cliente = entity.Cliente is null ? null : new ClientePedidoResumenDataModel
            {
                id_cliente = entity.Cliente.id_cliente,
                nombre = entity.Cliente.nombre,
                apellidos = entity.Cliente.apellidos,
                correo = entity.Cliente.correo
            },
            MetodoPago = entity.MetodoPago is null ? null : new MetodoPagoPedidoResumenDataModel
            {
                id_metodo = entity.MetodoPago.id_metodo,
                met_nombre = entity.MetodoPago.met_nombre,
                codigo_sri = entity.MetodoPago.codigo_sri
            },
            Detalles = entity.Detalles.Select(ProductoPedidoDataMapper.ToDataModel).ToList()
        };

        public static PedidoEntity ToEntity(PedidoDataModel model)
        {
            var entity = new PedidoEntity
            {
                id_pedido = model.id_pedido,
                id_metodo = model.id_metodo,
                id_cliente = model.id_cliente,
                fecha_hora = model.fecha_hora,
                estado = model.estado,
                entrega_fisica = model.entrega_fisica,
                subtotal = model.subtotal,
                iva = model.iva,
                total = model.total,
                fecha_pago = model.fecha_pago
            };

            foreach (var detalleModel in model.Detalles)
            {
                var detalle = ProductoPedidoDataMapper.ToEntity(detalleModel);
                detalle.id_pedido = model.id_pedido;
                entity.Detalles.Add(detalle);
            }

            return entity;
        }

        public static void ApplyToEntity(PedidoDataModel model, PedidoEntity entity)
        {
            entity.id_metodo = model.id_metodo;
            entity.id_cliente = model.id_cliente;
            entity.fecha_hora = model.fecha_hora;
            entity.entrega_fisica = model.entrega_fisica;
            entity.subtotal = model.subtotal;
            entity.iva = model.iva;
            entity.total = model.total;
        }
    }
}
