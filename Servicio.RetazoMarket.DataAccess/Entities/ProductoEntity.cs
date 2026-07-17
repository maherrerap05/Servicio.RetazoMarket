using System.Text.Json;

namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ProductoEntity
    {
        public int id_producto { get; set; }
        public int id_linea { get; set; }
        public string prod_nombre { get; set; } = null!;
        public decimal prod_peso { get; set; }
        public string prod_descripcion { get; set; } = null!;
        public JsonDocument colores { get; set; } = null!;
        public decimal costo_mat_prim { get; set; }
        public decimal costo_mano_obra { get; set; }
        public decimal porcentaje_margen_ganancia { get; set; }
        public decimal precio_base { get; set; }
        public string es_personalizable { get; set; } = null!;
        public int stock_actual { get; set; }
        public int stock_descuento { get; set; }
        public string prod_estado { get; set; } = null!;

        public LineaEntity Linea { get; set; } = null!;
        public ICollection<ProductoMaterialEntity> Materiales { get; set; } = new List<ProductoMaterialEntity>();
        public ICollection<PersonalizacionEntity> Personalizaciones { get; set; } = new List<PersonalizacionEntity>();
        public ICollection<ImagenEntity> Imagenes { get; set; } = new List<ImagenEntity>();
        public ICollection<FavoritoEntity> Favoritos { get; set; } = new List<FavoritoEntity>();
        public ICollection<MovimientoProductoEntity> Movimientos { get; set; } = new List<MovimientoProductoEntity>();
        public ICollection<ProductoPedidoEntity> DetallesPedidos { get; set; } = new List<ProductoPedidoEntity>();
    }
}
