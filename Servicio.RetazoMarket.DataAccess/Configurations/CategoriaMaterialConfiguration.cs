using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class CategoriaMaterialConfiguration : IEntityTypeConfiguration<CategoriaMaterialEntity>
    {
        public void Configure(EntityTypeBuilder<CategoriaMaterialEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("cat_mat", "public", tb =>
            {
                tb.HasCheckConstraint("ck_cat_mat_estado", "cat_estado IN ('ACT','INA')");
            });

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(c => c.id_categoria)
                   .HasName("pk_cat_mat");

            builder.Property(c => c.id_categoria)
                   .HasColumnName("id_categoria")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_cat_mat_id'::regclass)");

            // =========================
            // CAMPOS PRINCIPALES
            // =========================
            builder.Property(c => c.cat_nombre)
                   .HasColumnName("cat_nombre")
                   .IsRequired()
                   .HasMaxLength(100);

            // =========================
            // ESTADO / CICLO DE VIDA
            // =========================
            builder.Property(c => c.cat_estado)
                   .HasColumnName("cat_estado")
                   .IsRequired()
                   .HasMaxLength(3)
                   .IsFixedLength();
        }
    }
}
