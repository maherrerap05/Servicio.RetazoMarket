namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class PedidoFiltroDataModel
    {
        public int? id_cliente { get; set; }
        public string? estado { get; set; }
        public DateTime? fecha_desde_utc { get; set; }
        public DateTime? fecha_hasta_utc { get; set; }
        public int? id_linea { get; set; }
        public int? id_metodo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
