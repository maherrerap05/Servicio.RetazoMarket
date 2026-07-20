using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class MovimientoProductoConfiguration : IEntityTypeConfiguration<MovimientoProductoEntity>
    {
        public void Configure(EntityTypeBuilder<MovimientoProductoEntity> builder)
        {
            builder.ToTable("mov_productos", "public", tb =>
            {
                tb.HasCheckConstraint("ck_mov_productos_tipo", "tipo_movimiento IN ('ING','EGR','AJP','AJN')");
                tb.HasCheckConstraint("ck_mov_productos_cantidad", "cantidad > 0");
            });

            builder.HasKey(m => m.id_movimiento2).HasName("pk_mov_productos");
            builder.Property(m => m.id_movimiento2).HasColumnName("id_movimiento2").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_mov_productos_id'::regclass)");
            builder.Property(m => m.id_producto).HasColumnName("id_producto").IsRequired();
            builder.Property(m => m.tipo_movimiento).HasColumnName("tipo_movimiento").IsRequired().HasMaxLength(3).IsFixedLength();
            builder.Property(m => m.cantidad).HasColumnName("cantidad").IsRequired();
            builder.Property(m => m.fecha_mov).HasColumnName("fecha_mov").IsRequired()
                .HasColumnType("timestamp without time zone")
                .HasConversion(value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
                    value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
            builder.Property(m => m.motivo_mov).HasColumnName("motivo_mov").IsRequired().HasMaxLength(100);

            builder.HasOne(m => m.Producto).WithMany(p => p.Movimientos).HasForeignKey(m => m.id_producto)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mov_prod_presenta_producto");
        }
    }
}
