using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ProductoDataService : IProductoDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<ProductoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProductoRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(ProductoDataMapper.ToDataModel).ToList();
        }

        public async Task<ProductoDataModel?> ObtenerPorIdAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoRepository.ObtenerPorIdAsync(id_producto, cancellationToken);
            return entity is null ? null : ProductoDataMapper.ToDataModel(entity);
        }

        public async Task<ProductoDataModel?> ObtenerPorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoRepository.ObtenerPorNombreAsync(prod_nombre, cancellationToken);
            return entity is null ? null : ProductoDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<ProductoDataModel>> BuscarAsync(ProductoFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ProductoQueryRepository.BuscarAsync(filtro.nombre, filtro.id_linea, filtro.estado,
                filtro.es_personalizable, filtro.conStock, filtro.precioMinimo, filtro.precioMaximo,
                filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, ProductoDataMapper.ToDataModel);
        }

        public async Task<ProductoDataModel> CrearAsync(ProductoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ProductoDataMapper.ToEntity(model);
            await _unitOfWork.ProductoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoDataMapper.ToDataModel(entity);
        }

        public async Task<ProductoDataModel?> ActualizarAsync(ProductoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoRepository.ObtenerParaActualizarAsync(model.id_producto, cancellationToken);
            if (entity is null) return null;
            ProductoDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ProductoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoRepository.ObtenerParaActualizarAsync(id_producto, cancellationToken);
            if (entity is null) return false;
            entity.prod_estado = "INA";
            _unitOfWork.ProductoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorNombreAsync(string prod_nombre, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProductoRepository.ExistePorNombreAsync(prod_nombre, cancellationToken);
    }
}
