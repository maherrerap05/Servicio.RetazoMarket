namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class FavoritoEntity
    {
        public int id_cliente { get; set; }
        public int id_producto { get; set; }

        public ClienteEntity Cliente { get; set; } = null!;
        public ProductoEntity Producto { get; set; } = null!;
    }
}
