using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly RetazoMarketDbContext _context;
        public PedidoRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<IReadOnlyList<PedidoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
            await _context.Pedidos.AsNoTracking().Include(p => p.Cliente).Include(p => p.MetodoPago)
                .OrderByDescending(p => p.fecha_hora).ThenByDescending(p => p.id_pedido).ToListAsync(cancellationToken);

        public async Task<PedidoEntity?> ObtenerPorIdAsync(int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.Pedidos.AsNoTracking().AsSplitQuery().Include(p => p.Cliente).Include(p => p.MetodoPago)
                .Include(p => p.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.id_pedido == id_pedido, cancellationToken);

        public async Task<PedidoEntity?> ObtenerParaActualizarAsync(int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.Pedidos.FirstOrDefaultAsync(p => p.id_pedido == id_pedido, cancellationToken);

        public async Task<PedidoEntity?> ObtenerParaPagarAsync(int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.Pedidos.AsSplitQuery().Include(p => p.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.id_pedido == id_pedido && p.estado == "PEN", cancellationToken);

        public async Task AgregarAsync(PedidoEntity pedido, CancellationToken cancellationToken = default) =>
            await _context.Pedidos.AddAsync(pedido, cancellationToken);

        public void Actualizar(PedidoEntity pedido) => _context.Pedidos.Update(pedido);
    }
}
