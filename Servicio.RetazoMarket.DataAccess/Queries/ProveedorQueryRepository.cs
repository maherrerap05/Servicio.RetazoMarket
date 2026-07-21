using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Common;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;

namespace Servicio.RetazoMarket.DataAccess.Queries
{
    public class ProveedorQueryRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProveedorQueryRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<PagedResult<ProveedorEntity>> BuscarAsync(
            string? codigoProveedor, string? nombre, string? correo, string? estado, int? id_material,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Proveedores.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(codigoProveedor))
                query = query.Where(p => EF.Functions.ILike(p.codigo_proveedor, $"%{codigoProveedor}%"));
            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => EF.Functions.ILike(p.prov_nombre, $"%{nombre}%"));
            if (!string.IsNullOrWhiteSpace(correo))
                query = query.Where(p => EF.Functions.ILike(p.prov_correo, $"%{correo}%"));
            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(p => p.prov_estado == estado);
            if (id_material.HasValue)
                query = query.Where(p => p.MaterialesSuministrados.Any(pm => pm.id_material == id_material.Value));

            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Include(p => p.MaterialesSuministrados).ThenInclude(pm => pm.Material)
                .OrderBy(p => p.prov_nombre).ThenBy(p => p.id_proveedor)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<ProveedorEntity>
            {
                Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords
            };
        }
    }
}
