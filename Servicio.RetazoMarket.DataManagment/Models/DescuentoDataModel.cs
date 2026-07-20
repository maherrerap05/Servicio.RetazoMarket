namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class DescuentoDataModel
    {
        public int id_descuento { get; set; }
        public int id_producto { get; set; }
        public int cantidad_minima { get; set; }
        public decimal porcentaje { get; set; }
        public string estado { get; set; } = null!;

        public ProductoDescuentoResumenDataModel? Producto { get; set; }
    }

    public class ProductoDescuentoResumenDataModel
    {
        public int id_producto { get; set; }
        public string prod_nombre { get; set; } = null!;
        public string prod_estado { get; set; } = null!;
        public string tiene_descuentos { get; set; } = null!;
    }
}
