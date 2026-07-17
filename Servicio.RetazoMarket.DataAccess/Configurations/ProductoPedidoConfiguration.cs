using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ProductoPedidoConfiguration : IEntityTypeConfiguration<ProductoPedidoEntity>
    {
        public void Configure(EntityTypeBuilder<ProductoPedidoEntity> builder)
        {
            builder.ToTable("pro_x_ped", "public", tb =>
            {
                tb.HasCheckConstraint("ck_pro_x_ped_cantidad", "cantidad > 0");
                tb.HasCheckConstraint("ck_pro_x_ped_montos", "precio_unitario >= 0 AND monto_descuento >= 0 AND subtotal_item >= 0");
                tb.HasCheckConstraint("ck_pro_x_ped_descuento", "porcentaje_descuento >= 0 AND porcentaje_descuento <= 100");
                tb.HasCheckConstraint("ck_pro_x_ped_subtotal_consistente", "subtotal_item = (precio_unitario * cantidad) - monto_descuento");
            });

            builder.HasKey(d => new { d.id_producto, d.id_pedido }).HasName("pk_pro_x_ped");
            builder.Property(d => d.id_producto).HasColumnName("id_producto");
            builder.Property(d => d.id_pedido).HasColumnName("id_pedido");
            builder.Property(d => d.cantidad).HasColumnName("cantidad").IsRequired();
            builder.Property(d => d.precio_unitario).HasColumnName("precio_unitario").IsRequired().HasPrecision(10, 2);
            builder.Property(d => d.porcentaje_descuento).HasColumnName("porcentaje_descuento").IsRequired().HasPrecision(10, 2);
            builder.Property(d => d.monto_descuento).HasColumnName("monto_descuento").IsRequired().HasPrecision(10, 2);
            builder.Property(d => d.subtotal_item).HasColumnName("subtotal_item").IsRequired().HasPrecision(10, 2);
            builder.Property(d => d.personalizacion_selec).HasColumnName("personalizacion_selec").IsRequired().HasColumnType("jsonb");

            builder.HasOne(d => d.Pedido).WithMany(p => p.Detalles).HasForeignKey(d => d.id_pedido)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pro_x_pe_pro_x_ped_pedidos");
            builder.HasOne(d => d.Producto).WithMany(p => p.DetallesPedidos).HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pro_x_pe_pro_x_ped_producto");
        }
    }
}
