using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly RetazoMarketDbContext _context;
        public MaterialRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<IReadOnlyList<MaterialEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
            await _context.Materiales.AsNoTracking().Include(m => m.Categoria).OrderBy(m => m.id_material).ToListAsync(cancellationToken);

        public async Task<MaterialEntity?> ObtenerPorIdAsync(int id_material, CancellationToken cancellationToken = default) =>
            await _context.Materiales.AsNoTracking().Include(m => m.Categoria).Include(m => m.Proveedores).ThenInclude(pm => pm.Proveedor)
                .FirstOrDefaultAsync(m => m.id_material == id_material, cancellationToken);

        public async Task<MaterialEntity?> ObtenerParaActualizarAsync(int id_material, CancellationToken cancellationToken = default) =>
            await _context.Materiales.FirstOrDefaultAsync(m => m.id_material == id_material, cancellationToken);

        public async Task<MaterialEntity?> ObtenerPorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default) =>
            await _context.Materiales.AsNoTracking().FirstOrDefaultAsync(m => m.mat_nombre == mat_nombre, cancellationToken);

        public async Task AgregarAsync(MaterialEntity material, CancellationToken cancellationToken = default) =>
            await _context.Materiales.AddAsync(material, cancellationToken);

        public void Actualizar(MaterialEntity material) => _context.Materiales.Update(material);

        public async Task<bool> ExistePorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default) =>
            await _context.Materiales.AsNoTracking().AnyAsync(m => m.mat_nombre == mat_nombre, cancellationToken);
    }
}
