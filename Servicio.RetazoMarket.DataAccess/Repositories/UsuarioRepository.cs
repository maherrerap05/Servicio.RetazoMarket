using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly RetazoMarketDbContext _context;

        public UsuarioRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================
        public async Task<IReadOnlyList<UsuarioEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .OrderBy(u => u.id_usuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<UsuarioEntity?> ObtenerPorIdAsync(int id_usuario, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.id_usuario == id_usuario, cancellationToken);
        }

        public async Task<UsuarioEntity?> ObtenerParaActualizarAsync(int id_usuario, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.id_usuario == id_usuario, cancellationToken);
        }

        public async Task<UsuarioEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.correo == correo, cancellationToken);
        }

        // =========================
        // COMANDOS
        // =========================
        public async Task AgregarAsync(UsuarioEntity usuario, CancellationToken cancellationToken = default)
        {
            await _context.Usuarios.AddAsync(usuario, cancellationToken);
        }

        public void Actualizar(UsuarioEntity usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.correo == correo, cancellationToken);
        }
    }
}
