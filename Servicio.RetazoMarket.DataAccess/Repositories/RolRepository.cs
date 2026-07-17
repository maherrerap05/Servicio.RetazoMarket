using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly RetazoMarketDbContext _context;

        public RolRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================
        public async Task<IReadOnlyList<RolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.id_rol)
                .ToListAsync(cancellationToken);
        }

        public async Task<RolEntity?> ObtenerPorIdAsync(int id_rol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.id_rol == id_rol, cancellationToken);
        }

        public async Task<RolEntity?> ObtenerParaActualizarAsync(int id_rol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.id_rol == id_rol, cancellationToken);
        }

        public async Task<RolEntity?> ObtenerPorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.nombre_rol == nombre_rol, cancellationToken);
        }

        // =========================
        // COMANDOS
        // =========================
        public async Task AgregarAsync(RolEntity rol, CancellationToken cancellationToken = default)
        {
            await _context.Roles.AddAsync(rol, cancellationToken);
        }

        public void Actualizar(RolEntity rol)
        {
            _context.Roles.Update(rol);
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .AnyAsync(r => r.nombre_rol == nombre_rol, cancellationToken);
        }
    }
}
