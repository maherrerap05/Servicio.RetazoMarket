using System.Data;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class MovimientoMaterialDataService : IMovimientoMaterialDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovimientoMaterialDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<MovimientoMaterialDataModel?> ObtenerPorIdAsync(int id_movimiento, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MovimientoMaterialRepository.ObtenerPorIdAsync(id_movimiento, cancellationToken);
            return entity is null ? null : MovimientoMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<MovimientoMaterialDataModel>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.MovimientoMaterialRepository.ObtenerPorMaterialAsync(id_material, cancellationToken);
            return entities.Select(MovimientoMaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<DataPagedResult<MovimientoMaterialDataModel>> BuscarAsync(
            MovimientoMaterialFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.MovimientoMaterialQueryRepository.BuscarAsync(
                filtro.id_material, filtro.tipo_movimiento, filtro.motivo,
                filtro.fecha_desde_utc, filtro.fecha_hasta_utc,
                filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, MovimientoMaterialDataMapper.ToDataModel);
        }

        public async Task<MovimientoMaterialDataModel> CrearAsync(
            MovimientoMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = MovimientoMaterialDataMapper.ToEntity(model);
            await _unitOfWork.MovimientoMaterialRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MovimientoMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<MovimientoMaterialDataModel?> CrearConActualizacionStockAsync(
            MovimientoMaterialDataModel model, decimal stock_resultante, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var material = await _unitOfWork.MaterialRepository.ObtenerParaActualizarAsync(model.id_material, cancellationToken);
                if (material is null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return null;
                }

                material.stock_actual = stock_resultante;
                _unitOfWork.MaterialRepository.Actualizar(material);

                var movimiento = MovimientoMaterialDataMapper.ToEntity(model);
                await _unitOfWork.MovimientoMaterialRepository.AgregarAsync(movimiento, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return MovimientoMaterialDataMapper.ToDataModel(movimiento);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                throw;
            }
        }
    }
}
