using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class DescuentoDataService : IDescuentoDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DescuentoDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DescuentoDataModel?> ObtenerPorIdAsync(
            int id_descuento,
            CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.DescuentoRepository
                .ObtenerPorIdAsync(id_descuento, cancellationToken);

            return entity is null ? null : DescuentoDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<DescuentoDataModel>> ObtenerTodosAsync(
            CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.DescuentoRepository
                .ObtenerTodosAsync(cancellationToken);

            return entities.Select(DescuentoDataMapper.ToDataModel).ToList();
        }

        public async Task<IReadOnlyList<DescuentoDataModel>> ObtenerPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.DescuentoRepository
                .ObtenerPorProductoAsync(id_producto, cancellationToken);

            return entities.Select(DescuentoDataMapper.ToDataModel).ToList();
        }

        public async Task<DataPagedResult<DescuentoDataModel>> BuscarAsync(
            DescuentoFiltroDataModel filtro,
            CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.DescuentoQueryRepository.BuscarAsync(
                filtro.id_producto,
                filtro.cantidad_minima,
                filtro.porcentaje,
                filtro.estado,
                filtro.PageNumber,
                filtro.PageSize,
                cancellationToken);

            return DataPagedResultMapper.ToDataPagedResult(
                result,
                DescuentoDataMapper.ToDataModel);
        }

        public async Task<IReadOnlyList<DescuentoDataModel>> ObtenerActivosPorProductoAsync(
            int id_producto,
            CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.DescuentoQueryRepository
                .ObtenerActivosPorProductoAsync(id_producto, cancellationToken);

            return entities.Select(DescuentoDataMapper.ToDataModel).ToList();
        }

        public async Task<DescuentoDataModel?> ObtenerAplicableAsync(
            int id_producto,
            int cantidad_comprada,
            CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.DescuentoQueryRepository
                .ObtenerAplicableAsync(id_producto, cantidad_comprada, cancellationToken);

            return entity is null ? null : DescuentoDataMapper.ToDataModel(entity);
        }

        public async Task<DescuentoDataModel> CrearAsync(
            DescuentoDataModel model,
            CancellationToken cancellationToken = default)
        {
            var entity = DescuentoDataMapper.ToEntity(model);
            await _unitOfWork.DescuentoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return DescuentoDataMapper.ToDataModel(entity);
        }

        public async Task<DescuentoDataModel?> ActualizarAsync(
            DescuentoDataModel model,
            CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.DescuentoRepository
                .ObtenerParaActualizarAsync(model.id_descuento, cancellationToken);

            if (entity is null)
                return null;

            DescuentoDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.DescuentoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return DescuentoDataMapper.ToDataModel(entity);
        }

        public async Task<bool> InactivarAsync(
            int id_descuento,
            CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.DescuentoRepository
                .ObtenerParaActualizarAsync(id_descuento, cancellationToken);

            if (entity is null)
                return false;

            entity.estado = "INA";
            _unitOfWork.DescuentoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorProductoYCantidadMinimaAsync(
            int id_producto,
            int cantidad_minima,
            int? id_descuento_excluir = null,
            CancellationToken cancellationToken = default)
        {
            return _unitOfWork.DescuentoRepository.ExistePorProductoYCantidadMinimaAsync(
                id_producto,
                cantidad_minima,
                id_descuento_excluir,
                cancellationToken);
        }
    }
}
