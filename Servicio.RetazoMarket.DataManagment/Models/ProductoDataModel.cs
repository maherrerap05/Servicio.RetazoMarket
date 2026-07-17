using System.Text.Json;

namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProductoDataModel
    {
        public int id_producto { get; set; }
        public int id_linea { get; set; }
        public string prod_nombre { get; set; } = null!;
        public decimal prod_peso { get; set; }
        public string prod_descripcion { get; set; } = null!;
        public JsonElement colores { get; set; }
        public decimal costo_mat_prim { get; set; }
        public decimal costo_mano_obra { get; set; }
        public decimal porcentaje_margen_ganancia { get; set; }
        public decimal precio_base { get; set; }
        public string es_personalizable { get; set; } = null!;
        public int stock_actual { get; set; }
        public int stock_descuento { get; set; }
        public string prod_estado { get; set; } = null!;
        public LineaProductoResumenDataModel? Linea { get; set; }
        public IReadOnlyCollection<ProductoMaterialResumenDataModel> Materiales { get; set; } = Array.Empty<ProductoMaterialResumenDataModel>();
        public IReadOnlyCollection<PersonalizacionResumenDataModel> Personalizaciones { get; set; } = Array.Empty<PersonalizacionResumenDataModel>();
        public IReadOnlyCollection<ImagenResumenDataModel> Imagenes { get; set; } = Array.Empty<ImagenResumenDataModel>();
    }

    public class LineaProductoResumenDataModel
    {
        public int id_linea { get; set; }
        public string? lin_nombre { get; set; }
        public string? lin_estado { get; set; }
    }

    public class ProductoMaterialResumenDataModel
    {
        public int id_material { get; set; }
        public string mat_nombre { get; set; } = null!;
        public string unidad_medida { get; set; } = null!;
        public int cantidad_req { get; set; }
        public string es_personalizable { get; set; } = null!;
    }

    public class PersonalizacionResumenDataModel
    {
        public int id_opcion { get; set; }
        public string nombre_atr { get; set; } = null!;
        public string tipo_valor { get; set; } = null!;
        public JsonElement valores_json { get; set; }
        public decimal costo_adicional { get; set; }
    }

    public class ImagenResumenDataModel
    {
        public int id_imagen { get; set; }
        public string url { get; set; } = null!;
        public string es_principal { get; set; } = null!;
        public int orden { get; set; }
    }
}
