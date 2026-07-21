using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProveedorRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<IReadOnlyList<ProveedorEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking().OrderBy(p => p.id_proveedor).ToListAsync(cancellationToken);

        public async Task<ProveedorEntity?> ObtenerPorIdAsync(int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking().Include(p => p.MaterialesSuministrados).ThenInclude(pm => pm.Material)
                .FirstOrDefaultAsync(p => p.id_proveedor == id_proveedor, cancellationToken);

        public async Task<ProveedorEntity?> ObtenerParaActualizarAsync(int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.FirstOrDefaultAsync(p => p.id_proveedor == id_proveedor, cancellationToken);

        public async Task<ProveedorEntity?> ObtenerPorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking()
                .FirstOrDefaultAsync(p => p.codigo_proveedor == codigo_proveedor, cancellationToken);

        public async Task<ProveedorEntity?> ObtenerPorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(p => p.prov_correo == prov_correo, cancellationToken);

        public async Task AgregarAsync(ProveedorEntity proveedor, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AddAsync(proveedor, cancellationToken);

        public void Actualizar(ProveedorEntity proveedor) => _context.Proveedores.Update(proveedor);

        public async Task<bool> ExistePorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking()
                .AnyAsync(p => p.codigo_proveedor == codigo_proveedor, cancellationToken);

        public async Task<bool> ExistePorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default) =>
            await _context.Proveedores.AsNoTracking().AnyAsync(p => p.prov_correo == prov_correo, cancellationToken);
    }
}
