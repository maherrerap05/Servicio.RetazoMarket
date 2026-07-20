using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class DescuentoConfiguration : IEntityTypeConfiguration<DescuentoEntity>
    {
        public void Configure(EntityTypeBuilder<DescuentoEntity> builder)
        {
            builder.ToTable("descuentos", "public", tb =>
            {
                tb.HasCheckConstraint("ck_descuentos_cantidad", "cantidad_minima > 0");
                tb.HasCheckConstraint("ck_descuentos_porcentaje", "porcentaje > 0 AND porcentaje <= 100");
                tb.HasCheckConstraint("ck_descuentos_estado", "estado IN ('ACT','INA')");
            });

            builder.HasKey(d => d.id_descuento)
                .HasName("pk_descuentos");

            builder.Property(d => d.id_descuento)
                .HasColumnName("id_descuento")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_descuentos_id'::regclass)");

            builder.Property(d => d.id_producto)
                .HasColumnName("id_producto")
                .IsRequired();

            builder.Property(d => d.cantidad_minima)
                .HasColumnName("cantidad_minima")
                .IsRequired();

            builder.Property(d => d.porcentaje)
                .HasColumnName("porcentaje")
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(d => d.estado)
                .HasColumnName("estado")
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .HasDefaultValue("ACT");

            builder.HasIndex(d => new { d.id_producto, d.cantidad_minima })
                .IsUnique()
                .HasDatabaseName("uq_descuentos_producto_cantidad");

            builder.HasIndex(d => d.id_producto)
                .HasDatabaseName("ix_descuentos_producto");

            builder.HasOne(d => d.Producto)
                .WithMany(p => p.Descuentos)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_descuentos_producto");
        }
    }
}
