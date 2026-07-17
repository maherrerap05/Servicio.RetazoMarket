using System.Data;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class MovimientoProductoDataService : IMovimientoProductoDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovimientoProductoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<MovimientoProductoDataModel?> ObtenerPorIdAsync(int id_movimiento2, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MovimientoProductoRepository.ObtenerPorIdAsync(id_movimiento2, cancellationToken);
            return entity is null ? null : MovimientoProductoDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<MovimientoProductoDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.MovimientoProductoRepository.ObtenerPorProductoAsync(id_producto, cancellationToken);
            return entities.Select(MovimientoProductoDataMapper.ToDataModel).ToList();
        }

        public async Task<DataPagedResult<MovimientoProductoDataModel>> BuscarAsync(
            MovimientoProductoFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.MovimientoProductoQueryRepository.BuscarAsync(
                filtro.id_producto, filtro.tipo_movimiento, filtro.motivo,
                filtro.fecha_desde_utc, filtro.fecha_hasta_utc,
                filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, MovimientoProductoDataMapper.ToDataModel);
        }

        public async Task<MovimientoProductoDataModel> CrearAsync(
            MovimientoProductoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = MovimientoProductoDataMapper.ToEntity(model);
            await _unitOfWork.MovimientoProductoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MovimientoProductoDataMapper.ToDataModel(entity);
        }

        public async Task<MovimientoProductoDataModel?> CrearConActualizacionStockAsync(
            MovimientoProductoDataModel model, int stock_resultante, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var producto = await _unitOfWork.ProductoRepository.ObtenerParaActualizarAsync(model.id_producto, cancellationToken);
                if (producto is null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return null;
                }

                producto.stock_actual = stock_resultante;
                _unitOfWork.ProductoRepository.Actualizar(producto);

                var movimiento = MovimientoProductoDataMapper.ToEntity(model);
                await _unitOfWork.MovimientoProductoRepository.AgregarAsync(movimiento, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return MovimientoProductoDataMapper.ToDataModel(movimiento);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                throw;
            }
        }
    }
}
