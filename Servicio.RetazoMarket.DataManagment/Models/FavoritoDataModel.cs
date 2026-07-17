using System.Text.Json;

namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class FavoritoDataModel
    {
        public int id_cliente { get; set; }
        public int id_producto { get; set; }
        public FavoritoProductoResumenDataModel? Producto { get; set; }
    }

    public class FavoritoProductoResumenDataModel
    {
        public int id_producto { get; set; }
        public int id_linea { get; set; }
        public string prod_nombre { get; set; } = null!;
        public string prod_descripcion { get; set; } = null!;
        public JsonElement colores { get; set; }
        public decimal precio_base { get; set; }
        public string es_personalizable { get; set; } = null!;
        public int stock_actual { get; set; }
        public string prod_estado { get; set; } = null!;
        public LineaFavoritoResumenDataModel? Linea { get; set; }
        public IReadOnlyCollection<ImagenFavoritoResumenDataModel> Imagenes { get; set; } = Array.Empty<ImagenFavoritoResumenDataModel>();
    }

    public class LineaFavoritoResumenDataModel
    {
        public int id_linea { get; set; }
        public string? lin_nombre { get; set; }
    }

    public class ImagenFavoritoResumenDataModel
    {
        public int id_imagen { get; set; }
        public string url { get; set; } = null!;
        public string es_principal { get; set; } = null!;
        public int orden { get; set; }
    }
}
