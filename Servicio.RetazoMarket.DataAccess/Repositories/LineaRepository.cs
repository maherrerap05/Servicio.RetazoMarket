using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class LineaRepository : ILineaRepository
    {
        private readonly RetazoMarketDbContext _context;

        public LineaRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<LineaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Lineas
                .AsNoTracking()
                .OrderBy(l => l.id_linea)
                .ToListAsync(cancellationToken);
        }

        public async Task<LineaEntity?> ObtenerPorIdAsync(int id_linea, CancellationToken cancellationToken = default)
        {
            return await _context.Lineas
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.id_linea == id_linea, cancellationToken);
        }

        public async Task<LineaEntity?> ObtenerParaActualizarAsync(int id_linea, CancellationToken cancellationToken = default)
        {
            return await _context.Lineas
                .FirstOrDefaultAsync(l => l.id_linea == id_linea, cancellationToken);
        }

        public async Task<LineaEntity?> ObtenerPorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Lineas
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.lin_nombre == lin_nombre, cancellationToken);
        }

        public async Task AgregarAsync(LineaEntity linea, CancellationToken cancellationToken = default)
        {
            await _context.Lineas.AddAsync(linea, cancellationToken);
        }

        public void Actualizar(LineaEntity linea)
        {
            _context.Lineas.Update(linea);
        }

        public async Task<bool> ExistePorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Lineas
                .AsNoTracking()
                .AnyAsync(l => l.lin_nombre == lin_nombre, cancellationToken);
        }
    }
}
