using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<MaterialEntity>
    {
        public void Configure(EntityTypeBuilder<MaterialEntity> builder)
        {
            builder.ToTable("materiales", "public", tb =>
            {
                tb.HasCheckConstraint("ck_materiales_stock", "stock_actual >= 0");
                tb.HasCheckConstraint("ck_materiales_estado", "mat_estado IN ('ACT','INA')");
            });

            builder.HasKey(m => m.id_material).HasName("pk_materiales");
            builder.Property(m => m.id_material).HasColumnName("id_material").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_materiales_id'::regclass)");
            builder.Property(m => m.id_categoria).HasColumnName("id_categoria").IsRequired();
            builder.Property(m => m.mat_nombre).HasColumnName("mat_nombre").IsRequired().HasMaxLength(100);
            builder.Property(m => m.unidad_medida).HasColumnName("unidad_medida").IsRequired().HasMaxLength(50);
            builder.Property(m => m.stock_actual).HasColumnName("stock_actual").IsRequired();
            builder.Property(m => m.mat_estado).HasColumnName("mat_estado").IsRequired().HasMaxLength(3).IsFixedLength();

            builder.HasOne(m => m.Categoria).WithMany(c => c.Materiales).HasForeignKey(m => m.id_categoria)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_material_agrupa_cat_mat");
        }
    }
}
