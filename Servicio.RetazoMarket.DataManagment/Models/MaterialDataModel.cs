namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MaterialDataModel
    {
        public int id_material { get; set; }
        public int id_categoria { get; set; }
        public string mat_nombre { get; set; } = null!;
        public string unidad_medida { get; set; } = null!;
        public decimal stock_actual { get; set; }
        public string mat_estado { get; set; } = null!;
        public CategoriaMaterialResumenDataModel? Categoria { get; set; }
        public IReadOnlyCollection<ProveedorMaterialResumenDataModel> Proveedores { get; set; } = Array.Empty<ProveedorMaterialResumenDataModel>();
    }

    public class CategoriaMaterialResumenDataModel
    {
        public int id_categoria { get; set; }
        public string cat_nombre { get; set; } = null!;
        public string cat_estado { get; set; } = null!;
    }

    public class ProveedorMaterialResumenDataModel
    {
        public int id_proveedor { get; set; }
        public string codigo_proveedor { get; set; } = null!;
        public string prov_nombre { get; set; } = null!;
        public string prov_estado { get; set; } = null!;
        public string origen { get; set; } = null!;
        public decimal precio_compra { get; set; }
        public int cantidad_min { get; set; }
        public int dias_entrega { get; set; }
    }
}
