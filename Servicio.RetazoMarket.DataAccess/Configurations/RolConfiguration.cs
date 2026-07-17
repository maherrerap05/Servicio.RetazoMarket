using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEntity>
    {
        public void Configure(EntityTypeBuilder<RolEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("rol", "public", tb =>
            {
                tb.HasCheckConstraint(
                    "ck_rol_nombre",
                    "nombre_rol IN ('SUPERADMINISTRADOR','ADMINISTRADOR','CLIENTE')");
            });

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(r => r.id_rol)
                   .HasName("pk_rol");

            builder.Property(r => r.id_rol)
                   .HasColumnName("id_rol")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_rol_id'::regclass)");

            // =========================
            // IDENTIFICACIÓN
            // =========================
            builder.Property(r => r.nombre_rol)
                   .HasColumnName("nombre_rol")
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
