using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProductoRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<IReadOnlyList<ProductoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
            await _context.Productos.AsNoTracking().Include(p => p.Linea).OrderBy(p => p.id_producto).ToListAsync(cancellationToken);

        public async Task<ProductoEntity?> ObtenerPorIdAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.Productos.AsNoTracking().AsSplitQuery().Include(p => p.Linea)
                .Include(p => p.Materiales).ThenInclude(pm => pm.Material)
                .Include(p => p.Personalizaciones).Include(p => p.Imagenes)
                .Include(p => p.Descuentos)
                .FirstOrDefaultAsync(p => p.id_producto == id_producto, cancellationToken);

        public async Task<ProductoEntity?> ObtenerParaActualizarAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.Productos.FirstOrDefaultAsync(p => p.id_producto == id_producto, cancellationToken);

        public async Task<ProductoEntity?> ObtenerPorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default) =>
            await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.prod_nombre == prod_nombre, cancellationToken);

        public async Task AgregarAsync(ProductoEntity producto, CancellationToken cancellationToken = default) =>
            await _context.Productos.AddAsync(producto, cancellationToken);

        public void Actualizar(ProductoEntity producto) => _context.Productos.Update(producto);

        public async Task<bool> ExistePorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default) =>
            await _context.Productos.AsNoTracking().AnyAsync(p => p.prod_nombre == prod_nombre, cancellationToken);
    }
}
