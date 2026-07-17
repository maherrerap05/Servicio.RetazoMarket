using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ProductoMaterialRepository : IProductoMaterialRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProductoMaterialRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<ProductoMaterialEntity?> ObtenerAsync(int id_producto, int id_material, CancellationToken cancellationToken = default) =>
            await _context.ProductosMateriales.AsNoTracking().Include(pm => pm.Material)
                .FirstOrDefaultAsync(pm => pm.id_producto == id_producto && pm.id_material == id_material, cancellationToken);
        public async Task<ProductoMaterialEntity?> ObtenerParaActualizarAsync(int id_producto, int id_material, CancellationToken cancellationToken = default) =>
            await _context.ProductosMateriales.FirstOrDefaultAsync(pm => pm.id_producto == id_producto && pm.id_material == id_material, cancellationToken);
        public async Task<IReadOnlyList<ProductoMaterialEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.ProductosMateriales.AsNoTracking().Include(pm => pm.Material).Where(pm => pm.id_producto == id_producto)
                .OrderBy(pm => pm.Material.mat_nombre).ToListAsync(cancellationToken);
        public async Task AgregarAsync(ProductoMaterialEntity productoMaterial, CancellationToken cancellationToken = default) =>
            await _context.ProductosMateriales.AddAsync(productoMaterial, cancellationToken);
        public void Actualizar(ProductoMaterialEntity productoMaterial) => _context.ProductosMateriales.Update(productoMaterial);
        public async Task<bool> ExisteAsync(int id_producto, int id_material, CancellationToken cancellationToken = default) =>
            await _context.ProductosMateriales.AsNoTracking().AnyAsync(pm => pm.id_producto == id_producto && pm.id_material == id_material, cancellationToken);
    }
}
