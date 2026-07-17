using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class MetodoPagoConfiguration : IEntityTypeConfiguration<MetodoPagoEntity>
    {
        public void Configure(EntityTypeBuilder<MetodoPagoEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("metodo_pago", "public");

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(m => m.id_metodo)
                   .HasName("pk_metodo_pago");

            builder.Property(m => m.id_metodo)
                   .HasColumnName("id_metodo")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_metodo_pago_id'::regclass)");

            // =========================
            // CAMPOS PRINCIPALES
            // =========================
            builder.Property(m => m.met_nombre)
                   .HasColumnName("met_nombre")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.codigo_sri)
                   .HasColumnName("codigo_sri")
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
