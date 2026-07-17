using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class MovimientoMaterialConfiguration : IEntityTypeConfiguration<MovimientoMaterialEntity>
    {
        public void Configure(EntityTypeBuilder<MovimientoMaterialEntity> builder)
        {
            builder.ToTable("mov_materiales", "public");

            builder.HasKey(m => m.id_movimiento).HasName("pk_mov_materiales");
            builder.Property(m => m.id_movimiento).HasColumnName("id_movimiento").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_mov_materiales_id'::regclass)");
            builder.Property(m => m.id_material).HasColumnName("id_material").IsRequired();
            builder.Property(m => m.tipo_movimiento).HasColumnName("tipo_movimiento").IsRequired().HasMaxLength(3).IsFixedLength();
            builder.Property(m => m.cantidad).HasColumnName("cantidad").IsRequired();
            builder.Property(m => m.fecha_mov).HasColumnName("fecha_mov").IsRequired()
                .HasColumnType("timestamp without time zone")
                .HasConversion(value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
                    value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
            builder.Property(m => m.motivo_mov).HasColumnName("motivo_mov").IsRequired().HasMaxLength(100);
            builder.Property(m => m.metodo_pago).HasColumnName("metodo_pago").HasMaxLength(50).IsRequired(false);

            builder.HasOne(m => m.Material).WithMany(m => m.Movimientos).HasForeignKey(m => m.id_material)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mov_mate_registra_material");
        }
    }
}
