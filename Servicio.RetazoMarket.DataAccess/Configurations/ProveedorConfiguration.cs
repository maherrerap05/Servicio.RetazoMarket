using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<ProveedorEntity>
    {
        public void Configure(EntityTypeBuilder<ProveedorEntity> builder)
        {
            builder.ToTable("proveedores", "public", tb =>
            {
                tb.HasCheckConstraint("ck_proveedores_estado", "prov_estado IN ('ACT','INA')");
                tb.HasCheckConstraint("ck_proveedores_telefono", "prov_telefono ~ '^[0-9]{10}$'");
                tb.HasCheckConstraint("ck_proveedores_correo", "prov_correo ~ '^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$'");
            });

            builder.HasKey(p => p.id_proveedor).HasName("pk_proveedores");
            builder.Property(p => p.id_proveedor).HasColumnName("id_proveedor").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_proveedores_id'::regclass)");
            builder.Property(p => p.prov_nombre).HasColumnName("prov_nombre").IsRequired().HasMaxLength(100);
            builder.Property(p => p.prov_telefono).HasColumnName("prov_telefono").IsRequired().HasMaxLength(10).IsFixedLength();
            builder.Property(p => p.prov_correo).HasColumnName("prov_correo").IsRequired().HasMaxLength(100);
            builder.Property(p => p.prov_direccion).HasColumnName("prov_direccion").IsRequired().HasMaxLength(255);
            builder.Property(p => p.prov_estado).HasColumnName("prov_estado").IsRequired().HasMaxLength(3).IsFixedLength();
        }
    }
}
