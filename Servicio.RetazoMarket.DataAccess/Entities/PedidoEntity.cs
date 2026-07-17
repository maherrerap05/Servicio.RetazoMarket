namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class PedidoEntity
    {
        public int id_pedido { get; set; }
        public int id_metodo { get; set; }
        public int id_cliente { get; set; }
        public DateTime fecha_hora { get; set; }
        public string estado { get; set; } = null!;
        public string entrega_fisica { get; set; } = null!;
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public DateTime? fecha_pago { get; set; }

        public MetodoPagoEntity MetodoPago { get; set; } = null!;
        public ClienteEntity Cliente { get; set; } = null!;
        public ICollection<ProductoPedidoEntity> Detalles { get; set; } = new List<ProductoPedidoEntity>();
    }
}
