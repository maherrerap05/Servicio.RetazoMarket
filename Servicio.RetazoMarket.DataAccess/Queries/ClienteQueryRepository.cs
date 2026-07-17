using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class ClienteQueryRepository
    {
        private readonly RetazoMarketDbContext _context;

        public ClienteQueryRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CLIENTES ACTIVOS
        // ============================================
        public async Task<IEnumerable<ClienteEntity>> ObtenerClientesActivosAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => c.cli_estado == "ACT")
                .OrderBy(c => c.apellidos)
                .ThenBy(c => c.nombre)
                .ThenBy(c => c.id_cliente)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // BÚSQUEDA PAGINADA DE CLIENTES
        // ============================================
        public async Task<PagedResult<ClienteEntity>> BuscarAsync(
            string? nombre,
            string? apellidos,
            string? correo,
            string? telefono,
            string? origen,
            string? estado,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Clientes
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(c => EF.Functions.ILike(c.nombre, $"%{nombre}%"));

            if (!string.IsNullOrWhiteSpace(apellidos))
                query = query.Where(c => c.apellidos != null && EF.Functions.ILike(c.apellidos, $"%{apellidos}%"));

            if (!string.IsNullOrWhiteSpace(correo))
                query = query.Where(c => EF.Functions.ILike(c.correo, $"%{correo}%"));

            if (!string.IsNullOrWhiteSpace(telefono))
                query = query.Where(c => c.telefono.Contains(telefono));

            if (!string.IsNullOrWhiteSpace(origen))
                query = query.Where(c => c.origen == origen);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(c => c.cli_estado == estado);

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.apellidos)
                .ThenBy(c => c.nombre)
                .ThenBy(c => c.id_cliente)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ClienteEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
