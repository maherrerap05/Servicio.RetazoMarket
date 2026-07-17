using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("cliente", "public", tb =>
            {
                tb.HasCheckConstraint("ck_cliente_origen", "origen IN ('MKT','FIS')");
                tb.HasCheckConstraint("ck_cliente_estado", "cli_estado IN ('ACT','INA')");
                tb.HasCheckConstraint("ck_cliente_telefono", "telefono ~ '^[0-9]{10}$'");
                tb.HasCheckConstraint("ck_cliente_correo", "correo ~ '^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$'");
            });

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(c => c.id_cliente)
                   .HasName("pk_cliente");

            builder.Property(c => c.id_cliente)
                   .HasColumnName("id_cliente")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_cliente_id'::regclass)");

            // =========================
            // INFORMACIÓN PERSONAL
            // =========================
            builder.Property(c => c.nombre)
                   .HasColumnName("nombre")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.apellidos)
                   .HasColumnName("apellidos")
                   .HasMaxLength(100);

            // =========================
            // INFORMACIÓN DE CONTACTO
            // =========================
            builder.Property(c => c.correo)
                   .HasColumnName("correo")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.telefono)
                   .HasColumnName("telefono")
                   .IsRequired()
                   .HasMaxLength(10)
                   .IsFixedLength();

            builder.Property(c => c.direccion)
                   .HasColumnName("direccion")
                   .IsRequired()
                   .HasMaxLength(255);

            // =========================
            // ORIGEN Y ESTADO
            // =========================
            builder.Property(c => c.origen)
                   .HasColumnName("origen")
                   .IsRequired()
                   .HasMaxLength(3)
                   .IsFixedLength();

            builder.Property(c => c.cli_estado)
                   .HasColumnName("cli_estado")
                   .IsRequired()
                   .HasMaxLength(3)
                   .IsFixedLength();

            // =========================
            // REGISTRO UTC
            // =========================
            builder.Property(c => c.fecha_registro)
                   .HasColumnName("fecha_registro")
                   .IsRequired()
                   .HasColumnType("timestamp without time zone")
                   .HasConversion(
                       value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
                       value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

            // =========================
            // ÍNDICES ÚNICOS
            // =========================
            builder.HasIndex(c => c.correo)
                   .IsUnique()
                   .HasDatabaseName("uq_cliente_correo");
        }
    }
}
