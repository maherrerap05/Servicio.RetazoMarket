using System.Text.Json;

namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProductoPedidoDataModel
    {
        public int id_producto { get; set; }
        public int id_pedido { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal porcentaje_descuento { get; set; }
        public decimal monto_descuento { get; set; }
        public decimal subtotal_item { get; set; }
        public JsonElement personalizacion_selec { get; set; }
        public ProductoPedidoResumenDataModel? Producto { get; set; }
    }

    public class ProductoPedidoResumenDataModel
    {
        public int id_producto { get; set; }
        public string prod_nombre { get; set; } = null!;
        public string prod_descripcion { get; set; } = null!;
    }
}
