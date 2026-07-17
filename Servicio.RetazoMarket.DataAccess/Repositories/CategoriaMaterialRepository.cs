using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class CategoriaMaterialRepository : ICategoriaMaterialRepository
    {
        private readonly RetazoMarketDbContext _context;

        public CategoriaMaterialRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CategoriaMaterialEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CategoriasMateriales
                .AsNoTracking()
                .OrderBy(c => c.id_categoria)
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoriaMaterialEntity?> ObtenerPorIdAsync(int id_categoria, CancellationToken cancellationToken = default)
        {
            return await _context.CategoriasMateriales
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.id_categoria == id_categoria, cancellationToken);
        }

        public async Task<CategoriaMaterialEntity?> ObtenerParaActualizarAsync(int id_categoria, CancellationToken cancellationToken = default)
        {
            return await _context.CategoriasMateriales
                .FirstOrDefaultAsync(c => c.id_categoria == id_categoria, cancellationToken);
        }

        public async Task<CategoriaMaterialEntity?> ObtenerPorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.CategoriasMateriales
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.cat_nombre == cat_nombre, cancellationToken);
        }

        public async Task AgregarAsync(CategoriaMaterialEntity categoriaMaterial, CancellationToken cancellationToken = default)
        {
            await _context.CategoriasMateriales.AddAsync(categoriaMaterial, cancellationToken);
        }

        public void Actualizar(CategoriaMaterialEntity categoriaMaterial)
        {
            _context.CategoriasMateriales.Update(categoriaMaterial);
        }

        public async Task<bool> ExistePorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.CategoriasMateriales
                .AsNoTracking()
                .AnyAsync(c => c.cat_nombre == cat_nombre, cancellationToken);
        }
    }
}
