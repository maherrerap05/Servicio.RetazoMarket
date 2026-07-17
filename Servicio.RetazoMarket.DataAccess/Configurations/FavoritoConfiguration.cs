using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Configurations
{
    public class FavoritoConfiguration : IEntityTypeConfiguration<FavoritoEntity>
    {
        public void Configure(EntityTypeBuilder<FavoritoEntity> builder)
        {
            builder.ToTable("favoritos", "public");

            builder.HasKey(f => new { f.id_cliente, f.id_producto })
                   .HasName("pk_favoritos");

            builder.Property(f => f.id_cliente)
                   .HasColumnName("id_cliente");

            builder.Property(f => f.id_producto)
                   .HasColumnName("id_producto");

            builder.HasOne(f => f.Cliente)
                   .WithMany(c => c.Favoritos)
                   .HasForeignKey(f => f.id_cliente)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_favorito_favoritos_cliente");

            builder.HasOne(f => f.Producto)
                   .WithMany(p => p.Favoritos)
                   .HasForeignKey(f => f.id_producto)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_favorito_favoritos_producto");
        }
    }
}
