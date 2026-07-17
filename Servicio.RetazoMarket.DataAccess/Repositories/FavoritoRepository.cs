using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly RetazoMarketDbContext _context;

        public FavoritoRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<FavoritoEntity?> ObtenerAsync(
            int id_cliente,
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .Include(f => f.Producto)
                .FirstOrDefaultAsync(
                    f => f.id_cliente == id_cliente && f.id_producto == id_producto,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<FavoritoEntity>> ObtenerPorClienteAsync(
            int id_cliente,
            CancellationToken cancellationToken = default)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Linea)
                .Where(f => f.id_cliente == id_cliente)
                .OrderBy(f => f.Producto.prod_nombre)
                .ThenBy(f => f.id_producto)
                .ToListAsync(cancellationToken);
        }

        public async Task AgregarAsync(FavoritoEntity favorito, CancellationToken cancellationToken = default)
        {
            await _context.Favoritos.AddAsync(favorito, cancellationToken);
        }

        public void Eliminar(FavoritoEntity favorito)
        {
            _context.Favoritos.Remove(favorito);
        }

        public async Task<bool> ExisteAsync(
            int id_cliente,
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .AnyAsync(
                    f => f.id_cliente == id_cliente && f.id_producto == id_producto,
                    cancellationToken);
        }
    }
}
