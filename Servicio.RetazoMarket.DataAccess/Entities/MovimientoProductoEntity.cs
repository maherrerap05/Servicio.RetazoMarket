namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class MovimientoProductoEntity
    {
        public int id_movimiento2 { get; set; }
        public int id_producto { get; set; }
        public string tipo_movimiento { get; set; } = null!;
        public int cantidad { get; set; }
        public DateTime fecha_mov { get; set; }
        public string motivo_mov { get; set; } = null!;

        public ProductoEntity Producto { get; set; } = null!;
    }
}
