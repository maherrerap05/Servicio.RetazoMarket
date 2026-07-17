using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class PedidoQueryRepository
    {
        private readonly RetazoMarketDbContext _context;
        public PedidoQueryRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<PagedResult<PedidoEntity>> BuscarAsync(
            int? id_cliente, string? estado, DateTime? fecha_desde_utc, DateTime? fecha_hasta_utc,
            int? id_linea, int? id_metodo, int pageNumber, int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Pedidos.AsNoTracking().AsQueryable();
            if (id_cliente.HasValue) query = query.Where(p => p.id_cliente == id_cliente.Value);
            if (!string.IsNullOrWhiteSpace(estado)) query = query.Where(p => p.estado == estado);
            if (fecha_desde_utc.HasValue) query = query.Where(p => p.fecha_hora >= fecha_desde_utc.Value);
            if (fecha_hasta_utc.HasValue) query = query.Where(p => p.fecha_hora <= fecha_hasta_utc.Value);
            if (id_linea.HasValue) query = query.Where(p => p.Detalles.Any(d => d.Producto.id_linea == id_linea.Value));
            if (id_metodo.HasValue) query = query.Where(p => p.id_metodo == id_metodo.Value);

            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Include(p => p.Cliente).Include(p => p.MetodoPago)
                .OrderByDescending(p => p.fecha_hora).ThenByDescending(p => p.id_pedido)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedResult<PedidoEntity>
            { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords };
        }
    }
}
