namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MovimientoMaterialDataModel
    {
        public int id_movimiento { get; set; }
        public int id_material { get; set; }
        public string tipo_movimiento { get; set; } = null!;
        public decimal cantidad { get; set; }
        public DateTime fecha_mov { get; set; }
        public string motivo_mov { get; set; } = null!;
        public string? metodo_pago { get; set; }
        public MaterialMovimientoResumenDataModel? Material { get; set; }
    }

    public class MaterialMovimientoResumenDataModel
    {
        public int id_material { get; set; }
        public string mat_nombre { get; set; } = null!;
        public string unidad_medida { get; set; } = null!;
        public decimal stock_actual { get; set; }
    }
}
