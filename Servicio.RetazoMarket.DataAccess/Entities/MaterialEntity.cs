namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class MaterialEntity
    {
        public int id_material { get; set; }
        public int id_categoria { get; set; }
        public string mat_nombre { get; set; } = null!;
        public string unidad_medida { get; set; } = null!;
        public int stock_actual { get; set; }
        public string mat_estado { get; set; } = null!;

        public CategoriaMaterialEntity Categoria { get; set; } = null!;
        public ICollection<ProveedorMaterialEntity> Proveedores { get; set; } = new List<ProveedorMaterialEntity>();
        public ICollection<ProductoMaterialEntity> Productos { get; set; } = new List<ProductoMaterialEntity>();
        public ICollection<MovimientoMaterialEntity> Movimientos { get; set; } = new List<MovimientoMaterialEntity>();
    }
}
