using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class MovimientoMaterialRepository : IMovimientoMaterialRepository
    {
        private readonly RetazoMarketDbContext _context;
        public MovimientoMaterialRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<MovimientoMaterialEntity?> ObtenerPorIdAsync(int id_movimiento, CancellationToken cancellationToken = default) =>
            await _context.MovimientosMateriales.AsNoTracking().Include(m => m.Material)
                .FirstOrDefaultAsync(m => m.id_movimiento == id_movimiento, cancellationToken);

        public async Task<IReadOnlyList<MovimientoMaterialEntity>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default) =>
            await _context.MovimientosMateriales.AsNoTracking().Where(m => m.id_material == id_material)
                .OrderByDescending(m => m.fecha_mov).ThenByDescending(m => m.id_movimiento).ToListAsync(cancellationToken);

        public async Task AgregarAsync(MovimientoMaterialEntity movimiento, CancellationToken cancellationToken = default) =>
            await _context.MovimientosMateriales.AddAsync(movimiento, cancellationToken);
    }
}
