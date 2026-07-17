using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class FavoritoQueryRepository
    {
        private readonly RetazoMarketDbContext _context;

        public FavoritoQueryRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FavoritoEntity>> BuscarPorClienteAsync(
            int id_cliente,
            string? nombre_producto,
            int? id_linea,
            string? estado_producto,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Favoritos
                .AsNoTracking()
                .Where(f => f.id_cliente == id_cliente);

            if (!string.IsNullOrWhiteSpace(nombre_producto))
                query = query.Where(f => EF.Functions.ILike(f.Producto.prod_nombre, $"%{nombre_producto}%"));

            if (id_linea.HasValue)
                query = query.Where(f => f.Producto.id_linea == id_linea.Value);

            if (!string.IsNullOrWhiteSpace(estado_producto))
                query = query.Where(f => f.Producto.prod_estado == estado_producto);

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Linea)
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Imagenes)
                .OrderBy(f => f.Producto.prod_nombre)
                .ThenBy(f => f.id_producto)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<FavoritoEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
