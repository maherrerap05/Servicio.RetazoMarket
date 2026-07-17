using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ProductoMaterialDataService : IProductoMaterialDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductoMaterialDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ProductoMaterialDataModel?> ObtenerAsync(int id_producto, int id_material, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoMaterialRepository.ObtenerAsync(id_producto, id_material, cancellationToken);
            return entity is null ? null : ProductoMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<ProductoMaterialDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProductoMaterialRepository.ObtenerPorProductoAsync(id_producto, cancellationToken);
            return entities.Select(ProductoMaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<ProductoMaterialDataModel> CrearAsync(ProductoMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ProductoMaterialDataMapper.ToEntity(model);
            await _unitOfWork.ProductoMaterialRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<ProductoMaterialDataModel?> ActualizarAsync(ProductoMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoMaterialRepository.ObtenerParaActualizarAsync(
                model.id_producto, model.id_material, cancellationToken);
            if (entity is null) return null;
            ProductoMaterialDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ProductoMaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoMaterialDataMapper.ToDataModel(entity);
        }

        public Task<bool> ExisteAsync(int id_producto, int id_material, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProductoMaterialRepository.ExisteAsync(id_producto, id_material, cancellationToken);
    }
}
