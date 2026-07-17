namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProveedorDataModel
    {
        public int id_proveedor { get; set; }
        public string prov_nombre { get; set; } = null!;
        public string prov_telefono { get; set; } = null!;
        public string prov_correo { get; set; } = null!;
        public string prov_direccion { get; set; } = null!;
        public string prov_estado { get; set; } = null!;
        public IReadOnlyCollection<MaterialProveedorResumenDataModel> Materiales { get; set; } = Array.Empty<MaterialProveedorResumenDataModel>();
    }

    public class MaterialProveedorResumenDataModel
    {
        public int id_material { get; set; }
        public string mat_nombre { get; set; } = null!;
        public string unidad_medida { get; set; } = null!;
        public string origen { get; set; } = null!;
        public decimal precio_compra { get; set; }
        public int cantidad_min { get; set; }
        public int dias_entrega { get; set; }
    }
}
