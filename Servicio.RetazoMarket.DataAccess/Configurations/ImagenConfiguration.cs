using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class ImagenConfiguration : IEntityTypeConfiguration<ImagenEntity>
    {
        public void Configure(EntityTypeBuilder<ImagenEntity> builder)
        {
            builder.ToTable("imagenes", "public", tb =>
            {
                tb.HasCheckConstraint("ck_imagenes_principal", "es_principal IN ('S','N')");
                tb.HasCheckConstraint("ck_imagenes_orden", "orden >= 0");
            });

            builder.HasKey(i => i.id_imagen).HasName("pk_imagenes");
            builder.Property(i => i.id_imagen).HasColumnName("id_imagen").ValueGeneratedOnAdd()
                .HasDefaultValueSql("nextval('seq_imagenes_id'::regclass)");
            builder.Property(i => i.id_producto).HasColumnName("id_producto").IsRequired();
            builder.Property(i => i.url).HasColumnName("url").IsRequired().HasMaxLength(500);
            builder.Property(i => i.es_principal).HasColumnName("es_principal").IsRequired().HasMaxLength(1).IsFixedLength();
            builder.Property(i => i.orden).HasColumnName("orden").IsRequired();

            builder.HasOne(i => i.Producto).WithMany(p => p.Imagenes).HasForeignKey(i => i.id_producto)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_imagenes_muestra_producto");
        }
    }
}
