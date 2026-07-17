namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class MovimientoMaterialEntity
    {
        public int id_movimiento { get; set; }
        public int id_material { get; set; }
        public string tipo_movimiento { get; set; } = null!;
        public int cantidad { get; set; }
        public DateTime fecha_mov { get; set; }
        public string motivo_mov { get; set; } = null!;
        public string? metodo_pago { get; set; }

        public MaterialEntity Material { get; set; } = null!;
    }
}
