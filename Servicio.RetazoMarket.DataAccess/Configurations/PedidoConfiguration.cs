using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<PedidoEntity>
    {
        public void Configure(EntityTypeBuilder<PedidoEntity> builder)
        {
            builder.ToTable("pedidos", "public", tb =>
            {
                tb.HasCheckConstraint("ck_pedidos_montos", "subtotal >= 0 AND iva >= 0 AND total >= 0");
                tb.HasCheckConstraint("ck_pedidos_total_consistente", "total = subtotal + iva");
                tb.HasCheckConstraint("ck_pedidos_estado", "estado IN ('PEN','REA')");
                tb.HasCheckConstraint("ck_pedidos_entrega_fisica", "entrega_fisica IN ('S','N')");
            });

            var utcConverter = new ValueConverter<DateTime, DateTime>(
                value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
                value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

            builder.HasKey(p => p.id_pedido).HasName("pk_pedidos");
            builder.Property(p => p.id_pedido).HasColumnName("id_pedido").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_pedidos_id'::regclass)");
            builder.Property(p => p.id_metodo).HasColumnName("id_metodo").IsRequired();
            builder.Property(p => p.id_cliente).HasColumnName("id_cliente").IsRequired();
            builder.Property(p => p.fecha_hora).HasColumnName("fecha_hora").IsRequired()
                .HasColumnType("timestamp without time zone").HasConversion(utcConverter);
            builder.Property(p => p.estado).HasColumnName("estado").IsRequired().HasMaxLength(3).IsFixedLength();
            builder.Property(p => p.entrega_fisica).HasColumnName("entrega_fisica").IsRequired().HasMaxLength(1)
                .IsFixedLength().HasDefaultValue("N");
            builder.Property(p => p.subtotal).HasColumnName("subtotal").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.iva).HasColumnName("iva").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.total).HasColumnName("total").IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.fecha_pago).HasColumnName("fecha_pago").IsRequired(false)
                .HasColumnType("timestamp without time zone").HasConversion(utcConverter);

            builder.HasOne(p => p.MetodoPago).WithMany(m => m.Pedidos).HasForeignKey(p => p.id_metodo)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pedidos_correspon_metodo_p");
            builder.HasOne(p => p.Cliente).WithMany(c => c.Pedidos).HasForeignKey(p => p.id_cliente)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pedidos_realiza_cliente");
        }
    }
}
