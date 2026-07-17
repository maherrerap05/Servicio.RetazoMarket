namespace Servicio.RetazoMarket.DataManagment.Models
{
    public enum EstadoPagoSimuladoData
    {
        Exitoso,
        PedidoNoEncontrado,
        PedidoNoPendiente,
        PedidoSinDetalles,
        StockInsuficiente,
        ConflictoConcurrencia,
        ErrorPersistencia
    }

    public class PagoSimuladoDataResult
    {
        public EstadoPagoSimuladoData Estado { get; set; }
        public int id_pedido { get; set; }
        public DateTime? fecha_pago_utc { get; set; }
        public IReadOnlyCollection<StockInsuficienteDataModel> ProductosSinStock { get; set; } = Array.Empty<StockInsuficienteDataModel>();
        public bool Exitoso => Estado == EstadoPagoSimuladoData.Exitoso;
    }

    public class StockInsuficienteDataModel
    {
        public int id_producto { get; set; }
        public int stock_disponible { get; set; }
        public int cantidad_requerida { get; set; }
    }
}
