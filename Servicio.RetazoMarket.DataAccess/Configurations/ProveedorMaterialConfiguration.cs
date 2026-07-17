using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ProveedorMaterialConfiguration : IEntityTypeConfiguration<ProveedorMaterialEntity>
    {
        public void Configure(EntityTypeBuilder<ProveedorMaterialEntity> builder)
        {
            builder.ToTable("prv_x_mat", "public", tb =>
            {
                tb.HasCheckConstraint("ck_prv_x_mat_origen", "origen IN ('NAC','IMP')");
                tb.HasCheckConstraint("ck_prv_x_mat_condiciones", "precio_compra >= 0 AND cantidad_min > 0 AND dias_entrega >= 0");
            });

            builder.HasKey(pm => new { pm.id_material, pm.id_proveedor }).HasName("pk_prv_x_mat");
            builder.Property(pm => pm.id_material).HasColumnName("id_material");
            builder.Property(pm => pm.id_proveedor).HasColumnName("id_proveedor");
            builder.Property(pm => pm.origen).HasColumnName("origen").IsRequired().HasMaxLength(3).IsFixedLength();
            builder.Property(pm => pm.precio_compra).HasColumnName("precio_compra").IsRequired().HasPrecision(10, 2);
            builder.Property(pm => pm.cantidad_min).HasColumnName("cantidad_min").IsRequired();
            builder.Property(pm => pm.dias_entrega).HasColumnName("dias_entrega").IsRequired();

            builder.HasOne(pm => pm.Material).WithMany(m => m.Proveedores).HasForeignKey(pm => pm.id_material)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_prv_x_ma_prv_x_mat_material");
            builder.HasOne(pm => pm.Proveedor).WithMany(p => p.MaterialesSuministrados).HasForeignKey(pm => pm.id_proveedor)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_prv_x_ma_prv_x_mat_proveedo");
        }
    }
}
