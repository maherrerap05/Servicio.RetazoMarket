using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Queries;
using Servicio.RetazoMarket.DataAccess.Repositories;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using System.Data;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RetazoMarketDbContext _context;
        private IDbContextTransaction? _activeTransaction;
        private bool _disposed;

        // =========================
        // REPOSITORIES
        // =========================
        public IRolRepository RolRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public IUsuarioRepository UsuarioRepository { get; }
        public ICategoriaMaterialRepository CategoriaMaterialRepository { get; }
        public ILineaRepository LineaRepository { get; }
        public IMetodoPagoRepository MetodoPagoRepository { get; }
        public IProveedorRepository ProveedorRepository { get; }
        public IMaterialRepository MaterialRepository { get; }
        public IProveedorMaterialRepository ProveedorMaterialRepository { get; }
        public IProductoRepository ProductoRepository { get; }
        public IProductoMaterialRepository ProductoMaterialRepository { get; }
        public IPersonalizacionRepository PersonalizacionRepository { get; }
        public IImagenRepository ImagenRepository { get; }
        public IFavoritoRepository FavoritoRepository { get; }
        public IMovimientoMaterialRepository MovimientoMaterialRepository { get; }
        public IMovimientoProductoRepository MovimientoProductoRepository { get; }
        public IPedidoRepository PedidoRepository { get; }
        public IProductoPedidoRepository ProductoPedidoRepository { get; }
        public IDescuentoRepository DescuentoRepository { get; }

        // =========================
        // QUERY REPOSITORIES
        // =========================
        public RolQueryRepository RolQueryRepository { get; }
        public ClienteQueryRepository ClienteQueryRepository { get; }
        public UsuarioQueryRepository UsuarioQueryRepository { get; }
        public ProveedorQueryRepository ProveedorQueryRepository { get; }
        public MaterialQueryRepository MaterialQueryRepository { get; }
        public ProductoQueryRepository ProductoQueryRepository { get; }
        public FavoritoQueryRepository FavoritoQueryRepository { get; }
        public MovimientoMaterialQueryRepository MovimientoMaterialQueryRepository { get; }
        public MovimientoProductoQueryRepository MovimientoProductoQueryRepository { get; }
        public PedidoQueryRepository PedidoQueryRepository { get; }
        public DescuentoQueryRepository DescuentoQueryRepository { get; }

        public UnitOfWork(RetazoMarketDbContext context)
        {
            _context = context;

            RolRepository = new RolRepository(_context);
            ClienteRepository = new ClienteRepository(_context);
            UsuarioRepository = new UsuarioRepository(_context);
            CategoriaMaterialRepository = new CategoriaMaterialRepository(_context);
            LineaRepository = new LineaRepository(_context);
            MetodoPagoRepository = new MetodoPagoRepository(_context);
            ProveedorRepository = new ProveedorRepository(_context);
            MaterialRepository = new MaterialRepository(_context);
            ProveedorMaterialRepository = new ProveedorMaterialRepository(_context);
            ProductoRepository = new ProductoRepository(_context);
            ProductoMaterialRepository = new ProductoMaterialRepository(_context);
            PersonalizacionRepository = new PersonalizacionRepository(_context);
            ImagenRepository = new ImagenRepository(_context);
            FavoritoRepository = new FavoritoRepository(_context);
            MovimientoMaterialRepository = new MovimientoMaterialRepository(_context);
            MovimientoProductoRepository = new MovimientoProductoRepository(_context);
            PedidoRepository = new PedidoRepository(_context);
            ProductoPedidoRepository = new ProductoPedidoRepository(_context);
            DescuentoRepository = new DescuentoRepository(_context);

            RolQueryRepository = new RolQueryRepository(_context);
            ClienteQueryRepository = new ClienteQueryRepository(_context);
            UsuarioQueryRepository = new UsuarioQueryRepository(_context);
            ProveedorQueryRepository = new ProveedorQueryRepository(_context);
            MaterialQueryRepository = new MaterialQueryRepository(_context);
            ProductoQueryRepository = new ProductoQueryRepository(_context);
            FavoritoQueryRepository = new FavoritoQueryRepository(_context);
            MovimientoMaterialQueryRepository = new MovimientoMaterialQueryRepository(_context);
            MovimientoProductoQueryRepository = new MovimientoProductoQueryRepository(_context);
            PedidoQueryRepository = new PedidoQueryRepository(_context);
            DescuentoQueryRepository = new DescuentoQueryRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void DescartarCambios()
        {
            ThrowIfDisposed();
            _context.ChangeTracker.Clear();
        }

        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_activeTransaction is not null)
                throw new InvalidOperationException("Ya existe una transacción activa en esta unidad de trabajo.");

            _activeTransaction = await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var transaction = _activeTransaction
                ?? throw new InvalidOperationException("No existe una transacción activa para confirmar.");

            await transaction.CommitAsync(cancellationToken);
            await DisposeActiveTransactionAsync();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var transaction = _activeTransaction
                ?? throw new InvalidOperationException("No existe una transacción activa para revertir.");

            await transaction.RollbackAsync(cancellationToken);
            await DisposeActiveTransactionAsync();
        }

        private async Task DisposeActiveTransactionAsync()
        {
            if (_activeTransaction is null)
                return;

            await _activeTransaction.DisposeAsync();
            _activeTransaction = null;
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _activeTransaction?.Dispose();
            _activeTransaction = null;
            _context.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            await DisposeActiveTransactionAsync();
            await _context.DisposeAsync();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
