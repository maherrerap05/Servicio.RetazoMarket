using System.Data;
using Servicio.RetazoMarket.DataAccess.Queries;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        // =========================
        // REPOSITORIES
        // =========================
        IRolRepository RolRepository { get; }
        IClienteRepository ClienteRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        ICategoriaMaterialRepository CategoriaMaterialRepository { get; }
        ILineaRepository LineaRepository { get; }
        IMetodoPagoRepository MetodoPagoRepository { get; }
        IProveedorRepository ProveedorRepository { get; }
        IMaterialRepository MaterialRepository { get; }
        IProveedorMaterialRepository ProveedorMaterialRepository { get; }
        IProductoRepository ProductoRepository { get; }
        IProductoMaterialRepository ProductoMaterialRepository { get; }
        IPersonalizacionRepository PersonalizacionRepository { get; }
        IImagenRepository ImagenRepository { get; }
        IFavoritoRepository FavoritoRepository { get; }
        IMovimientoMaterialRepository MovimientoMaterialRepository { get; }
        IMovimientoProductoRepository MovimientoProductoRepository { get; }
        IPedidoRepository PedidoRepository { get; }
        IProductoPedidoRepository ProductoPedidoRepository { get; }

        // =========================
        // QUERY REPOSITORIES
        // =========================
        RolQueryRepository RolQueryRepository { get; }
        ClienteQueryRepository ClienteQueryRepository { get; }
        UsuarioQueryRepository UsuarioQueryRepository { get; }
        ProveedorQueryRepository ProveedorQueryRepository { get; }
        MaterialQueryRepository MaterialQueryRepository { get; }
        ProductoQueryRepository ProductoQueryRepository { get; }
        FavoritoQueryRepository FavoritoQueryRepository { get; }
        MovimientoMaterialQueryRepository MovimientoMaterialQueryRepository { get; }
        MovimientoProductoQueryRepository MovimientoProductoQueryRepository { get; }
        PedidoQueryRepository PedidoQueryRepository { get; }

        // =========================
        // SAVE CHANGES
        // =========================
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void DescartarCambios();

        // =========================
        // TRANSACTIONS
        // =========================
        Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
