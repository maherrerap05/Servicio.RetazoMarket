namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProveedorMaterialDataModel
    {
        public int id_material { get; set; }
        public int id_proveedor { get; set; }
        public string origen { get; set; } = null!;
        public decimal precio_compra { get; set; }
        public int cantidad_min { get; set; }
        public int dias_entrega { get; set; }
        public string? material_nombre { get; set; }
        public string? proveedor_nombre { get; set; }
    }
}
