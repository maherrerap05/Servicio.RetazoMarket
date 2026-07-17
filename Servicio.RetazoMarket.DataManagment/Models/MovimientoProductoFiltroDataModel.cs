namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MovimientoProductoFiltroDataModel
    {
        public int? id_producto { get; set; }
        public string? tipo_movimiento { get; set; }
        public string? motivo { get; set; }
        public DateTime? fecha_desde_utc { get; set; }
        public DateTime? fecha_hasta_utc { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
