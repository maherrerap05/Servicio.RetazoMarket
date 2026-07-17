using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class PersonalizacionConfiguration : IEntityTypeConfiguration<PersonalizacionEntity>
    {
        public void Configure(EntityTypeBuilder<PersonalizacionEntity> builder)
        {
            builder.ToTable("personalizacion", "public", tb =>
            {
                tb.HasCheckConstraint("ck_personalizacion_costo", "costo_adicional >= 0");
                tb.HasCheckConstraint("ck_personalizacion_tipo_valor", "tipo_valor IN ('LISTA','MATERIAL')");
            });

            builder.HasKey(p => p.id_opcion).HasName("pk_personalizacion");
            builder.Property(p => p.id_opcion).HasColumnName("id_opcion").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_personalizacion_id'::regclass)");
            builder.Property(p => p.id_producto).HasColumnName("id_producto").IsRequired();
            builder.Property(p => p.nombre_atr).HasColumnName("nombre_atr").IsRequired().HasMaxLength(100);
            builder.Property(p => p.tipo_valor).HasColumnName("tipo_valor").IsRequired().HasMaxLength(10).IsFixedLength();
            builder.Property(p => p.valores_json).HasColumnName("valores_json").IsRequired().HasColumnType("jsonb");
            builder.Property(p => p.costo_adicional).HasColumnName("costo_adicional").IsRequired().HasPrecision(10, 2);

            builder.HasOne(p => p.Producto).WithMany(p => p.Personalizaciones).HasForeignKey(p => p.id_producto)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_personal_tiene_producto");
        }
    }
}
