using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<ProductoEntity>
    {
        public void Configure(EntityTypeBuilder<ProductoEntity> builder)
        {
            builder.ToTable("productos", "public", tb =>
            {
                tb.HasCheckConstraint("ck_productos_estado", "prod_estado IN ('ACT','INA')");
                tb.HasCheckConstraint("ck_productos_personalizable", "es_personalizable IN ('S','N')");
                tb.HasCheckConstraint("ck_productos_costos_positivos", "costo_mat_prim >= 0 AND costo_mano_obra >= 0 AND precio_base >= 0 AND prod_peso > 0");
                tb.HasCheckConstraint("ck_productos_stock", "stock_actual >= 0 AND stock_descuento >= 0");
                tb.HasCheckConstraint("ck_productos_margen", "porcentaje_margen_ganancia = 50");
            });

            builder.HasKey(p => p.id_producto).HasName("pk_productos");
            builder.Property(p => p.id_producto).HasColumnName("id_producto").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_productos_id'::regclass)");
            builder.Property(p => p.id_linea).HasColumnName("id_linea").IsRequired();
            builder.Property(p => p.prod_nombre).HasColumnName("prod_nombre").IsRequired().HasMaxLength(100);
            builder.Property(p => p.prod_peso).HasColumnName("prod_peso").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.prod_descripcion).HasColumnName("prod_descripcion").IsRequired().HasMaxLength(500);
            builder.Property(p => p.colores).HasColumnName("colores").IsRequired().HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb");
            builder.Property(p => p.costo_mat_prim).HasColumnName("costo_mat_prim").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.costo_mano_obra).HasColumnName("costo_mano_obra").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.porcentaje_margen_ganancia).HasColumnName("porcentaje_margen_ganancia").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.precio_base).HasColumnName("precio_base").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.es_personalizable).HasColumnName("es_personalizable").IsRequired().HasMaxLength(1).IsFixedLength();
            builder.Property(p => p.stock_actual).HasColumnName("stock_actual").IsRequired();
            builder.Property(p => p.stock_descuento).HasColumnName("stock_descuento").IsRequired();
            builder.Property(p => p.prod_estado).HasColumnName("prod_estado").IsRequired().HasMaxLength(3).IsFixedLength();

            builder.HasOne(p => p.Linea).WithMany(l => l.Productos).HasForeignKey(p => p.id_linea)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_producto_pertenece_lineas");
        }
    }
}
