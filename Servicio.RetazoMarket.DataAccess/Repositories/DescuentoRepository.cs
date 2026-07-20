using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class DescuentoRepository : IDescuentoRepository
    {
        private readonly RetazoMarketDbContext _context;

        public DescuentoRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DescuentoEntity>> ObtenerTodosAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .AsNoTracking()
                .Include(d => d.Producto)
                .OrderBy(d => d.id_producto)
                .ThenBy(d => d.cantidad_minima)
                .ThenBy(d => d.id_descuento)
                .ToListAsync(cancellationToken);
        }

        public async Task<DescuentoEntity?> ObtenerPorIdAsync(
            int id_descuento,
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .AsNoTracking()
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(
                    d => d.id_descuento == id_descuento,
                    cancellationToken);
        }

        public async Task<DescuentoEntity?> ObtenerParaActualizarAsync(
            int id_descuento,
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .FirstOrDefaultAsync(
                    d => d.id_descuento == id_descuento,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<DescuentoEntity>> ObtenerPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            return await _context.Descuentos
                .AsNoTracking()
                .Where(d => d.id_producto == id_producto)
                .OrderBy(d => d.cantidad_minima)
                .ThenBy(d => d.id_descuento)
                .ToListAsync(cancellationToken);
        }

        public async Task AgregarAsync(
            DescuentoEntity descuento,
            CancellationToken cancellationToken = default)
        {
            await _context.Descuentos.AddAsync(descuento, cancellationToken);
        }

        public void Actualizar(DescuentoEntity descuento)
        {
            _context.Descuentos.Update(descuento);
        }

        public async Task<bool> ExistePorProductoYCantidadMinimaAsync(
            int id_producto,
            int cantidad_minima,
            int? id_descuento_excluir = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Descuentos
                .AsNoTracking()
                .Where(d =>
                    d.id_producto == id_producto &&
                    d.cantidad_minima == cantidad_minima);

            if (id_descuento_excluir.HasValue)
                query = query.Where(d => d.id_descuento != id_descuento_excluir.Value);

            return await query.AnyAsync(cancellationToken);
        }
    }
}
