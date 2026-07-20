using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class ProductoQueryRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProductoQueryRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<PagedResult<ProductoEntity>> BuscarAsync(
            string? nombre, int? id_linea, string? estado, string? es_personalizable,
            string? tiene_descuentos, bool? conStock, decimal? precioMinimo, decimal? precioMaximo,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Productos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => EF.Functions.ILike(p.prod_nombre, $"%{nombre}%"));
            if (id_linea.HasValue)
                query = query.Where(p => p.id_linea == id_linea.Value);
            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(p => p.prod_estado == estado);
            if (!string.IsNullOrWhiteSpace(es_personalizable))
                query = query.Where(p => p.es_personalizable == es_personalizable);
            if (!string.IsNullOrWhiteSpace(tiene_descuentos))
                query = query.Where(p => p.tiene_descuentos == tiene_descuentos);
            if (conStock.HasValue)
                query = conStock.Value ? query.Where(p => p.stock_actual > 0) : query.Where(p => p.stock_actual == 0);
            if (precioMinimo.HasValue)
                query = query.Where(p => p.precio_base >= precioMinimo.Value);
            if (precioMaximo.HasValue)
                query = query.Where(p => p.precio_base <= precioMaximo.Value);

            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Include(p => p.Linea).Include(p => p.Imagenes)
                .OrderBy(p => p.prod_nombre).ThenBy(p => p.id_producto)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<ProductoEntity>
            {
                Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords
            };
        }
    }
}
