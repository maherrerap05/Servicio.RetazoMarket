using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class RolQueryRepository
    {
        private readonly RetazoMarketDbContext _context;

        public RolQueryRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // ============================================
        // BÚSQUEDA PAGINADA DE ROLES
        // ============================================
        public async Task<PagedResult<RolEntity>> BuscarAsync(
            string? nombre_rol,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Roles
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre_rol))
                query = query.Where(r => EF.Functions.ILike(r.nombre_rol, $"%{nombre_rol}%"));

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(r => r.nombre_rol)
                .ThenBy(r => r.id_rol)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<RolEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
