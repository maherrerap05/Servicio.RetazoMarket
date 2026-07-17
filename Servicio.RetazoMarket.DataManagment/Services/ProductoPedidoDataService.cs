using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ProductoPedidoDataService : IProductoPedidoDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductoPedidoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ProductoPedidoDataModel?> ObtenerAsync(
            int id_producto, int id_pedido, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProductoPedidoRepository.ObtenerAsync(id_producto, id_pedido, cancellationToken);
            return entity is null ? null : ProductoPedidoDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<ProductoPedidoDataModel>> ObtenerPorPedidoAsync(
            int id_pedido, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProductoPedidoRepository.ObtenerPorPedidoAsync(id_pedido, cancellationToken);
            return entities.Select(ProductoPedidoDataMapper.ToDataModel).ToList();
        }

        public async Task<ProductoPedidoDataModel> CrearAsync(
            ProductoPedidoDataModel model, CancellationToken cancellationToken = default)
        {
            var pedido = await _unitOfWork.PedidoRepository.ObtenerParaActualizarAsync(model.id_pedido, cancellationToken);
            if (pedido is null || pedido.estado != "PEN")
                throw new InvalidOperationException("Solo se pueden agregar detalles a pedidos pendientes.");

            var entity = ProductoPedidoDataMapper.ToEntity(model);
            await _unitOfWork.ProductoPedidoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoPedidoDataMapper.ToDataModel(entity);
        }

        public async Task<ProductoPedidoDataModel?> ActualizarAsync(
            ProductoPedidoDataModel model, CancellationToken cancellationToken = default)
        {
            var pedido = await _unitOfWork.PedidoRepository.ObtenerParaActualizarAsync(model.id_pedido, cancellationToken);
            if (pedido is null || pedido.estado != "PEN") return null;

            var entity = await _unitOfWork.ProductoPedidoRepository.ObtenerParaActualizarAsync(
                model.id_producto, model.id_pedido, cancellationToken);
            if (entity is null) return null;
            ProductoPedidoDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ProductoPedidoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProductoPedidoDataMapper.ToDataModel(entity);
        }

        public Task<bool> ExisteAsync(int id_producto, int id_pedido, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProductoPedidoRepository.ExisteAsync(id_producto, id_pedido, cancellationToken);
    }
}
