using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class MaterialQueryRepository
    {
        private readonly RetazoMarketDbContext _context;
        public MaterialQueryRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<PagedResult<MaterialEntity>> BuscarAsync(
            string? nombre, int? id_categoria, string? estado, int? stockMinimo, int? stockMaximo,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Materiales.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(m => EF.Functions.ILike(m.mat_nombre, $"%{nombre}%"));
            if (id_categoria.HasValue)
                query = query.Where(m => m.id_categoria == id_categoria.Value);
            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(m => m.mat_estado == estado);
            if (stockMinimo.HasValue)
                query = query.Where(m => m.stock_actual >= stockMinimo.Value);
            if (stockMaximo.HasValue)
                query = query.Where(m => m.stock_actual <= stockMaximo.Value);

            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Include(m => m.Categoria)
                .OrderBy(m => m.mat_nombre).ThenBy(m => m.id_material)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<MaterialEntity>
            {
                Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords
            };
        }
    }
}
