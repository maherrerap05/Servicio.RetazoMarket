using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ProductoMaterialConfiguration : IEntityTypeConfiguration<ProductoMaterialEntity>
    {
        public void Configure(EntityTypeBuilder<ProductoMaterialEntity> builder)
        {
            builder.ToTable("pro_x_mat", "public", tb =>
            {
                tb.HasCheckConstraint("ck_pro_x_mat_cantidad", "cantidad_req > 0");
                tb.HasCheckConstraint("ck_pro_x_mat_personalizable", "es_personalizable IN ('S','N')");
            });

            builder.HasKey(pm => new { pm.id_producto, pm.id_material }).HasName("pk_pro_x_mat");
            builder.Property(pm => pm.id_producto).HasColumnName("id_producto");
            builder.Property(pm => pm.id_material).HasColumnName("id_material");
            builder.Property(pm => pm.cantidad_req).HasColumnName("cantidad_req").IsRequired().HasPrecision(12, 3);
            builder.Property(pm => pm.es_personalizable).HasColumnName("es_personalizable").IsRequired().HasMaxLength(1).IsFixedLength();

            builder.HasOne(pm => pm.Producto).WithMany(p => p.Materiales).HasForeignKey(pm => pm.id_producto)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pro_x_ma_pro_x_mat_producto");
            builder.HasOne(pm => pm.Material).WithMany(m => m.Productos).HasForeignKey(pm => pm.id_material)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pro_x_ma_pro_x_mat_material");
        }
    }
}
