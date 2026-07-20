using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class DescuentoQueryRepository
    {
        private readonly RetazoMarketDbContext _context;

        public DescuentoQueryRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DescuentoEntity>> BuscarAsync(
            int? id_producto,
            int? cantidad_minima,
            decimal? porcentaje,
            string? estado,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Descuentos
                .AsNoTracking()
                .AsQueryable();

            if (id_producto.HasValue)
                query = query.Where(d => d.id_producto == id_producto.Value);

            if (cantidad_minima.HasValue)
                query = query.Where(d => d.cantidad_minima == cantidad_minima.Value);

            if (porcentaje.HasValue)
                query = query.Where(d => d.porcentaje == porcentaje.Value);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(d => d.estado == estado);

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .Include(d => d.Producto)
                .OrderBy(d => d.Producto.prod_nombre)
                .ThenBy(d => d.cantidad_minima)
                .ThenBy(d => d.id_descuento)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<DescuentoEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IReadOnlyList<DescuentoEntity>> ObtenerActivosPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .AsNoTracking()
                .Where(d =>
                    d.id_producto == id_producto &&
                    d.estado == "ACT")
                .OrderBy(d => d.cantidad_minima)
                .ThenBy(d => d.id_descuento)
                .ToListAsync(cancellationToken);
        }

        public async Task<DescuentoEntity?> ObtenerAplicableAsync(
            int id_producto,
            int cantidad_comprada,
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .AsNoTracking()
                .Where(d =>
                    d.id_producto == id_producto &&
                    d.estado == "ACT" &&
                    d.cantidad_minima <= cantidad_comprada)
                .OrderByDescending(d => d.cantidad_minima)
                .ThenByDescending(d => d.id_descuento)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
