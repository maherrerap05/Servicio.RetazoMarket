namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class PedidoDataModel
    {
        public int id_pedido { get; set; }
        public int id_metodo { get; set; }
        public int id_cliente { get; set; }
        public DateTime fecha_hora { get; set; }
        public string estado { get; set; } = null!;
        public string entrega_fisica { get; set; } = "N";
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public DateTime? fecha_pago { get; set; }
        public ClientePedidoResumenDataModel? Cliente { get; set; }
        public MetodoPagoPedidoResumenDataModel? MetodoPago { get; set; }
        public IReadOnlyCollection<ProductoPedidoDataModel> Detalles { get; set; } = Array.Empty<ProductoPedidoDataModel>();
    }

    public class ClientePedidoResumenDataModel
    {
        public int id_cliente { get; set; }
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string correo { get; set; } = null!;
    }

    public class MetodoPagoPedidoResumenDataModel
    {
        public int id_metodo { get; set; }
        public string met_nombre { get; set; } = null!;
        public string codigo_sri { get; set; } = null!;
    }
}
