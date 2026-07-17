namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MovimientoProductoDataModel
    {
        public int id_movimiento2 { get; set; }
        public int id_producto { get; set; }
        public string tipo_movimiento { get; set; } = null!;
        public int cantidad { get; set; }
        public DateTime fecha_mov { get; set; }
        public string motivo_mov { get; set; } = null!;
        public ProductoMovimientoResumenDataModel? Producto { get; set; }
    }

    public class ProductoMovimientoResumenDataModel
    {
        public int id_producto { get; set; }
        public string prod_nombre { get; set; } = null!;
        public int stock_actual { get; set; }
        public string prod_estado { get; set; } = null!;
    }
}
