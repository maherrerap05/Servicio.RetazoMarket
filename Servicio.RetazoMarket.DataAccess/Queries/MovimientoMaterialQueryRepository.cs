using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class MovimientoMaterialQueryRepository
    {
        private readonly RetazoMarketDbContext _context;
        public MovimientoMaterialQueryRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<PagedResult<MovimientoMaterialEntity>> BuscarAsync(
            int? id_material, string? tipo_movimiento, string? motivo,
            DateTime? fecha_desde_utc, DateTime? fecha_hasta_utc,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.MovimientosMateriales.AsNoTracking().AsQueryable();
            if (id_material.HasValue) query = query.Where(m => m.id_material == id_material.Value);
            if (!string.IsNullOrWhiteSpace(tipo_movimiento)) query = query.Where(m => m.tipo_movimiento == tipo_movimiento);
            if (!string.IsNullOrWhiteSpace(motivo)) query = query.Where(m => EF.Functions.ILike(m.motivo_mov, $"%{motivo}%"));
            if (fecha_desde_utc.HasValue) query = query.Where(m => m.fecha_mov >= fecha_desde_utc.Value);
            if (fecha_hasta_utc.HasValue) query = query.Where(m => m.fecha_mov <= fecha_hasta_utc.Value);

            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Include(m => m.Material).OrderByDescending(m => m.fecha_mov).ThenByDescending(m => m.id_movimiento)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedResult<MovimientoMaterialEntity>
            { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords };
        }
    }
}
