using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
        {
            // =========================
            // TABLA
            // =========================
            builder.ToTable("usuario", "public", tb =>
            {
                tb.HasCheckConstraint("ck_usuario_estado", "usr_estado IN ('ACT','INA')");
                tb.HasCheckConstraint("ck_usuario_correo", "correo ~ '^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$'");
            });

            // =========================
            // CLAVE PRIMARIA
            // =========================
            builder.HasKey(u => u.id_usuario)
                   .HasName("pk_usuario");

            builder.Property(u => u.id_usuario)
                   .HasColumnName("id_usuario")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("nextval('seq_usuario_id'::regclass)");

            // =========================
            // CLAVES FORÁNEAS
            // =========================
            builder.Property(u => u.id_cliente)
                   .HasColumnName("id_cliente")
                   .IsRequired(false);

            builder.Property(u => u.id_rol)
                   .HasColumnName("id_rol")
                   .IsRequired();

            // =========================
            // IDENTIFICACIÓN
            // =========================
            builder.Property(u => u.nombre)
                   .HasColumnName("nombre")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.correo)
                   .HasColumnName("correo")
                   .IsRequired()
                   .HasMaxLength(100);

            // =========================
            // SEGURIDAD
            // =========================
            builder.Property(u => u.contrasena_hash)
                   .HasColumnName("contrasena_hash")
                   .IsRequired()
                   .HasMaxLength(500);

            // =========================
            // ESTADO Y ACCESO UTC
            // =========================
            builder.Property(u => u.usr_estado)
                   .HasColumnName("usr_estado")
                   .IsRequired()
                   .HasMaxLength(3)
                   .IsFixedLength();

            builder.Property(u => u.ultimo_acceso)
                   .HasColumnName("ultimo_acceso")
                   .IsRequired()
                   .HasColumnType("timestamp without time zone")
                   .HasConversion(
                       value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
                       value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

            // =========================
            // ÍNDICES ÚNICOS
            // =========================
            builder.HasIndex(u => u.correo)
                   .IsUnique()
                   .HasDatabaseName("uq_usuario_correo");

            // =========================
            // RELACIONES
            // =========================
            builder.HasOne(u => u.Cliente)
                   .WithMany(c => c.Usuarios)
                   .HasForeignKey(u => u.id_cliente)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_usuario_tiene_2_cliente");

            builder.HasOne(u => u.Rol)
                   .WithMany(r => r.Usuarios)
                   .HasForeignKey(u => u.id_rol)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_usuario_represent_rol");
        }
    }
}
