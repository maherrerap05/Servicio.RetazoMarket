using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class LineaConfiguration : IEntityTypeConfiguration<LineaEntity>
    {
        public void Configure(EntityTypeBuilder<LineaEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("lineas", "public", tb =>
            {
                tb.HasCheckConstraint("ck_lineas_estado", "lin_estado IN ('ACT','INA')");
            });

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(l => l.id_linea)
                   .HasName("pk_lineas");

            builder.Property(l => l.id_linea)
                   .HasColumnName("id_linea")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_lineas_id'::regclass)");

            // =========================
            // CAMPOS PRINCIPALES
            // =========================
            builder.Property(l => l.lin_nombre)
                   .HasColumnName("lin_nombre")
                   .HasMaxLength(100)
                   .IsRequired(false);

            // =========================
            // ESTADO / CICLO DE VIDA
            // =========================
            builder.Property(l => l.lin_estado)
                   .HasColumnName("lin_estado")
                   .HasMaxLength(3)
                   .IsFixedLength()
                   .IsRequired(false);
        }
    }
}
