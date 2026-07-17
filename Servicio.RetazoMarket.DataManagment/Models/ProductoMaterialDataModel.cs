namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProductoMaterialDataModel
    {
        public int id_producto { get; set; }
        public int id_material { get; set; }
        public int cantidad_req { get; set; }
        public string es_personalizable { get; set; } = null!;
        public string? material_nombre { get; set; }
        public string? unidad_medida { get; set; }
    }
}
