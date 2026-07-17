using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly RetazoMarketDbContext _context;

        public MetodoPagoRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<MetodoPagoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .OrderBy(m => m.id_metodo)
                .ToListAsync(cancellationToken);
        }

        public async Task<MetodoPagoEntity?> ObtenerPorIdAsync(int id_metodo, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id_metodo == id_metodo, cancellationToken);
        }

        public async Task<MetodoPagoEntity?> ObtenerParaActualizarAsync(int id_metodo, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .FirstOrDefaultAsync(m => m.id_metodo == id_metodo, cancellationToken);
        }

        public async Task<MetodoPagoEntity?> ObtenerPorNombreAsync(string met_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.met_nombre == met_nombre, cancellationToken);
        }

        public async Task<MetodoPagoEntity?> ObtenerPorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.codigo_sri == codigo_sri, cancellationToken);
        }

        public async Task AgregarAsync(MetodoPagoEntity metodoPago, CancellationToken cancellationToken = default)
        {
            await _context.MetodosPago.AddAsync(metodoPago, cancellationToken);
        }

        public void Actualizar(MetodoPagoEntity metodoPago)
        {
            _context.MetodosPago.Update(metodoPago);
        }

        public async Task<bool> ExistePorNombreAsync(string met_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .AnyAsync(m => m.met_nombre == met_nombre, cancellationToken);
        }

        public async Task<bool> ExistePorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .AnyAsync(m => m.codigo_sri == codigo_sri, cancellationToken);
        }
    }
}
