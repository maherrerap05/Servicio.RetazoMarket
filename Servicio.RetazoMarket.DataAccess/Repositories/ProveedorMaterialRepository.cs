using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ProveedorMaterialRepository : IProveedorMaterialRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProveedorMaterialRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<ProveedorMaterialEntity?> ObtenerAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.AsNoTracking().Include(pm => pm.Material).Include(pm => pm.Proveedor)
                .FirstOrDefaultAsync(pm => pm.id_material == id_material && pm.id_proveedor == id_proveedor, cancellationToken);

        public async Task<ProveedorMaterialEntity?> ObtenerParaActualizarAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.FirstOrDefaultAsync(
                pm => pm.id_material == id_material && pm.id_proveedor == id_proveedor, cancellationToken);

        public async Task<IReadOnlyList<ProveedorMaterialEntity>> ObtenerPorProveedorAsync(int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.AsNoTracking().Include(pm => pm.Material)
                .Where(pm => pm.id_proveedor == id_proveedor).OrderBy(pm => pm.Material.mat_nombre).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<ProveedorMaterialEntity>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.AsNoTracking().Include(pm => pm.Proveedor)
                .Where(pm => pm.id_material == id_material).OrderBy(pm => pm.Proveedor.prov_nombre).ToListAsync(cancellationToken);

        public async Task AgregarAsync(ProveedorMaterialEntity proveedorMaterial, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.AddAsync(proveedorMaterial, cancellationToken);

        public void Actualizar(ProveedorMaterialEntity proveedorMaterial) => _context.ProveedoresMateriales.Update(proveedorMaterial);

        public async Task<bool> ExisteAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default) =>
            await _context.ProveedoresMateriales.AsNoTracking().AnyAsync(
                pm => pm.id_material == id_material && pm.id_proveedor == id_proveedor, cancellationToken);
    }
}
