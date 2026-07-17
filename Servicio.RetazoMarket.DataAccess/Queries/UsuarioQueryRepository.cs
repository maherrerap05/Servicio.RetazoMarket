using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class UsuarioQueryRepository
    {
        private readonly RetazoMarketDbContext _context;

        public UsuarioQueryRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // ============================================
        // USUARIOS ACTIVOS
        // ============================================
        public async Task<IEnumerable<UsuarioEntity>> ObtenerUsuariosActivosAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .Where(u => u.usr_estado == "ACT")
                .OrderBy(u => u.nombre)
                .ThenBy(u => u.id_usuario)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // BÚSQUEDA PAGINADA DE USUARIOS
        // ============================================
        public async Task<PagedResult<UsuarioEntity>> BuscarAsync(
            string? nombre,
            string? correo,
            string? estado,
            int? id_rol,
            int? id_cliente,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(u => EF.Functions.ILike(u.nombre, $"%{nombre}%"));

            if (!string.IsNullOrWhiteSpace(correo))
                query = query.Where(u => EF.Functions.ILike(u.correo, $"%{correo}%"));

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(u => u.usr_estado == estado);

            if (id_rol.HasValue)
                query = query.Where(u => u.id_rol == id_rol.Value);

            if (id_cliente.HasValue)
                query = query.Where(u => u.id_cliente == id_cliente.Value);

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(u => u.nombre)
                .ThenBy(u => u.id_usuario)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<UsuarioEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
