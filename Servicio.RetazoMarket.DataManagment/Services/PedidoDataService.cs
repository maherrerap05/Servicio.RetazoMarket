using System.Data;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class PedidoDataService : IPedidoDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PedidoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<PedidoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.PedidoRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(PedidoDataMapper.ToDataModel).ToList();
        }

        public async Task<PedidoDataModel?> ObtenerPorIdAsync(int id_pedido, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.PedidoRepository.ObtenerPorIdAsync(id_pedido, cancellationToken);
            return entity is null ? null : PedidoDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<PedidoDataModel>> BuscarAsync(
            PedidoFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PedidoQueryRepository.BuscarAsync(
                filtro.id_cliente, filtro.estado, filtro.fecha_desde_utc, filtro.fecha_hasta_utc,
                filtro.id_linea, filtro.id_metodo, filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, PedidoDataMapper.ToDataModel);
        }

        public async Task<PedidoDataModel> CrearAsync(PedidoDataModel model, CancellationToken cancellationToken = default)
        {
            if (model.Detalles.GroupBy(d => d.id_producto).Any(g => g.Count() > 1))
                throw new InvalidOperationException("Un producto no puede repetirse dentro del mismo pedido.");

            var entity = PedidoDataMapper.ToEntity(model);
            await _unitOfWork.PedidoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return PedidoDataMapper.ToDataModel(entity);
        }

        public async Task<PedidoDataModel?> ActualizarAsync(PedidoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.PedidoRepository.ObtenerParaActualizarAsync(model.id_pedido, cancellationToken);
            if (entity is null || entity.estado != "PEN") return null;
            PedidoDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.PedidoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return PedidoDataMapper.ToDataModel(entity);
        }

        public async Task<PagoSimuladoDataResult> PagarAsync(
            int id_pedido, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var pedido = await _unitOfWork.PedidoRepository.ObtenerParaPagarAsync(id_pedido, cancellationToken);
                if (pedido is null)
                {
                    var existente = await _unitOfWork.PedidoRepository.ObtenerPorIdAsync(id_pedido, cancellationToken);
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _unitOfWork.DescartarCambios();
                    return Resultado(existente is null
                        ? EstadoPagoSimuladoData.PedidoNoEncontrado
                        : EstadoPagoSimuladoData.PedidoNoPendiente, id_pedido);
                }

                if (pedido.Detalles.Count == 0)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _unitOfWork.DescartarCambios();
                    return Resultado(EstadoPagoSimuladoData.PedidoSinDetalles, id_pedido);
                }

                var faltantes = pedido.Detalles
                    .Where(d => d.Producto.stock_actual < d.cantidad)
                    .Select(d => new StockInsuficienteDataModel
                    {
                        id_producto = d.id_producto,
                        stock_disponible = d.Producto.stock_actual,
                        cantidad_requerida = d.cantidad
                    }).ToList();

                if (faltantes.Count > 0)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _unitOfWork.DescartarCambios();
                    return new PagoSimuladoDataResult
                    {
                        Estado = EstadoPagoSimuladoData.StockInsuficiente,
                        id_pedido = id_pedido,
                        ProductosSinStock = faltantes
                    };
                }

                var fechaPagoUtc = DateTime.UtcNow;
                foreach (var detalle in pedido.Detalles)
                {
                    detalle.Producto.stock_actual -= detalle.cantidad;
                    await _unitOfWork.MovimientoProductoRepository.AgregarAsync(
                        MovimientoProductoDataMapper.ToEntity(new MovimientoProductoDataModel
                        {
                            id_producto = detalle.id_producto,
                            tipo_movimiento = "EGR",
                            cantidad = detalle.cantidad,
                            fecha_mov = fechaPagoUtc,
                            motivo_mov = $"PAGO PEDIDO {id_pedido}"
                        }), cancellationToken);
                }

                pedido.fecha_pago = fechaPagoUtc;
                pedido.estado = "REA";

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return new PagoSimuladoDataResult
                {
                    Estado = EstadoPagoSimuladoData.Exitoso,
                    id_pedido = id_pedido,
                    fecha_pago_utc = fechaPagoUtc
                };
            }
            catch (OperationCanceledException)
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                _unitOfWork.DescartarCambios();
                throw;
            }
            catch (Exception exception)
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                _unitOfWork.DescartarCambios();
                return Resultado(EsConflictoConcurrencia(exception)
                    ? EstadoPagoSimuladoData.ConflictoConcurrencia
                    : EstadoPagoSimuladoData.ErrorPersistencia, id_pedido);
            }
        }

        private static PagoSimuladoDataResult Resultado(EstadoPagoSimuladoData estado, int idPedido) => new()
        {
            Estado = estado,
            id_pedido = idPedido
        };

        private static bool EsConflictoConcurrencia(Exception exception)
        {
            for (Exception? current = exception; current is not null; current = current.InnerException)
            {
                var sqlState = current.GetType().GetProperty("SqlState")?.GetValue(current) as string;
                if (sqlState is "40001" or "40P01") return true;
                if (current.GetType().Name.Contains("Concurrency", StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }
    }
}
