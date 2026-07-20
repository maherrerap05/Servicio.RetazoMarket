using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Configurations;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Context
{
    public class RetazoMarketDbContext : DbContext
    {
        public RetazoMarketDbContext(DbContextOptions<RetazoMarketDbContext> options)
            : base(options)
        {
        }

        // =========================
        // SEGURIDAD Y CLIENTES
        // =========================
        public DbSet<RolEntity> Roles => Set<RolEntity>();
        public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
        public DbSet<UsuarioEntity> Usuarios => Set<UsuarioEntity>();

        // =========================
        // CATÁLOGOS BASE
        // =========================
        public DbSet<CategoriaMaterialEntity> CategoriasMateriales => Set<CategoriaMaterialEntity>();
        public DbSet<LineaEntity> Lineas => Set<LineaEntity>();
        public DbSet<MetodoPagoEntity> MetodosPago => Set<MetodoPagoEntity>();

        // =========================
        // PROVEEDORES Y MATERIALES
        // =========================
        public DbSet<ProveedorEntity> Proveedores => Set<ProveedorEntity>();
        public DbSet<MaterialEntity> Materiales => Set<MaterialEntity>();
        public DbSet<ProveedorMaterialEntity> ProveedoresMateriales => Set<ProveedorMaterialEntity>();

        // =========================
        // CATÁLOGO DE PRODUCTOS
        // =========================
        public DbSet<ProductoEntity> Productos => Set<ProductoEntity>();
        public DbSet<ProductoMaterialEntity> ProductosMateriales => Set<ProductoMaterialEntity>();
        public DbSet<PersonalizacionEntity> Personalizaciones => Set<PersonalizacionEntity>();
        public DbSet<ImagenEntity> Imagenes => Set<ImagenEntity>();
        public DbSet<FavoritoEntity> Favoritos => Set<FavoritoEntity>();
        public DbSet<DescuentoEntity> Descuentos => Set<DescuentoEntity>();

        // =========================
        // MOVIMIENTOS DE INVENTARIO
        // =========================
        public DbSet<MovimientoMaterialEntity> MovimientosMateriales => Set<MovimientoMaterialEntity>();
        public DbSet<MovimientoProductoEntity> MovimientosProductos => Set<MovimientoProductoEntity>();

        // =========================
        // PEDIDOS
        // =========================
        public DbSet<PedidoEntity> Pedidos => Set<PedidoEntity>();
        public DbSet<ProductoPedidoEntity> ProductosPedidos => Set<ProductoPedidoEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new LineaConfiguration());
            modelBuilder.ApplyConfiguration(new MetodoPagoConfiguration());
            modelBuilder.ApplyConfiguration(new ProveedorConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ProveedorMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalizacionConfiguration());
            modelBuilder.ApplyConfiguration(new ImagenConfiguration());
            modelBuilder.ApplyConfiguration(new FavoritoConfiguration());
            modelBuilder.ApplyConfiguration(new DescuentoConfiguration());
            modelBuilder.ApplyConfiguration(new MovimientoMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new MovimientoProductoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoPedidoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
