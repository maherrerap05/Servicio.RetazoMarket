using System.Text.Json;

namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ProductoPedidoEntity
    {
        public int id_producto { get; set; }
        public int id_pedido { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal porcentaje_descuento { get; set; }
        public decimal monto_descuento { get; set; }
        public decimal subtotal_item { get; set; }
        public JsonDocument personalizacion_selec { get; set; } = null!;

        public ProductoEntity Producto { get; set; } = null!;
        public PedidoEntity Pedido { get; set; } = null!;
    }
}
