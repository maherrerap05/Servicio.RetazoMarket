using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ProductoPedidoRepository : IProductoPedidoRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ProductoPedidoRepository(RetazoMarketDbContext context) => _context = context;

        public async Task<ProductoPedidoEntity?> ObtenerAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.ProductosPedidos.AsNoTracking().Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.id_producto == id_producto && d.id_pedido == id_pedido, cancellationToken);

        public async Task<ProductoPedidoEntity?> ObtenerParaActualizarAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.ProductosPedidos.FirstOrDefaultAsync(
                d => d.id_producto == id_producto && d.id_pedido == id_pedido, cancellationToken);

        public async Task<IReadOnlyList<ProductoPedidoEntity>> ObtenerPorPedidoAsync(int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.ProductosPedidos.AsNoTracking().Include(d => d.Producto).Where(d => d.id_pedido == id_pedido)
                .OrderBy(d => d.Producto.prod_nombre).ThenBy(d => d.id_producto).ToListAsync(cancellationToken);

        public async Task AgregarAsync(ProductoPedidoEntity detalle, CancellationToken cancellationToken = default) =>
            await _context.ProductosPedidos.AddAsync(detalle, cancellationToken);

        public void Actualizar(ProductoPedidoEntity detalle) => _context.ProductosPedidos.Update(detalle);

        public async Task<bool> ExisteAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default) =>
            await _context.ProductosPedidos.AsNoTracking().AnyAsync(
                d => d.id_producto == id_producto && d.id_pedido == id_pedido, cancellationToken);
    }
}
