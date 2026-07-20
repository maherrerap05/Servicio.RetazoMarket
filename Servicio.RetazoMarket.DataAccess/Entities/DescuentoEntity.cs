namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class DescuentoEntity
    {
        public int id_descuento { get; set; }
        public int id_producto { get; set; }
        public int cantidad_minima { get; set; }
        public decimal porcentaje { get; set; }
        public string estado { get; set; } = null!;

        public ProductoEntity Producto { get; set; } = null!;
    }
}
