using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly RetazoMarketDbContext _context;

        public ClienteRepository(RetazoMarketDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================
        public async Task<IReadOnlyList<ClienteEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .OrderBy(c => c.id_cliente)
                .ToListAsync(cancellationToken);
        }

        public async Task<ClienteEntity?> ObtenerPorIdAsync(int id_cliente, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.id_cliente == id_cliente, cancellationToken);
        }

        public async Task<ClienteEntity?> ObtenerParaActualizarAsync(int id_cliente, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.id_cliente == id_cliente, cancellationToken);
        }

        public async Task<ClienteEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.correo == correo, cancellationToken);
        }

        // =========================
        // COMANDOS
        // =========================
        public async Task AgregarAsync(ClienteEntity cliente, CancellationToken cancellationToken = default)
        {
            await _context.Clientes.AddAsync(cliente, cancellationToken);
        }

        public void Actualizar(ClienteEntity cliente)
        {
            _context.Clientes.Update(cliente);
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .AnyAsync(c => c.correo == correo, cancellationToken);
        }
    }
}
