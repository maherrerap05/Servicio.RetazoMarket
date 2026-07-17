using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class MovimientoProductoRepository : IMovimientoProductoRepository
    {
        private readonly RetazoMarketDbContext _context;
        public MovimientoProductoRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<MovimientoProductoEntity?> ObtenerPorIdAsync(int id_movimiento2, CancellationToken cancellationToken = default) =>
            await _context.MovimientosProductos.AsNoTracking().Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.id_movimiento2 == id_movimiento2, cancellationToken);

        public async Task<IReadOnlyList<MovimientoProductoEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.MovimientosProductos.AsNoTracking().Where(m => m.id_producto == id_producto)
                .OrderByDescending(m => m.fecha_mov).ThenByDescending(m => m.id_movimiento2).ToListAsync(cancellationToken);

        public async Task AgregarAsync(MovimientoProductoEntity movimiento, CancellationToken cancellationToken = default) =>
            await _context.MovimientosProductos.AddAsync(movimiento, cancellationToken);
    }
}
