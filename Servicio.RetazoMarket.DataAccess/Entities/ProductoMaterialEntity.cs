namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ProductoMaterialEntity
    {
        public int id_producto { get; set; }
        public int id_material { get; set; }
        public int cantidad_req { get; set; }
        public string es_personalizable { get; set; } = null!;

        public ProductoEntity Producto { get; set; } = null!;
        public MaterialEntity Material { get; set; } = null!;
    }
}
