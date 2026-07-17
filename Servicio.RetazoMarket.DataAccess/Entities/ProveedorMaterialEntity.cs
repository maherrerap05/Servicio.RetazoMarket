namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ProveedorMaterialEntity
    {
        public int id_material { get; set; }
        public int id_proveedor { get; set; }
        public string origen { get; set; } = null!;
        public decimal precio_compra { get; set; }
        public int cantidad_min { get; set; }
        public int dias_entrega { get; set; }

        public MaterialEntity Material { get; set; } = null!;
        public ProveedorEntity Proveedor { get; set; } = null!;
    }
}
