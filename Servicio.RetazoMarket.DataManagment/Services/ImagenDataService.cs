using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ImagenDataService : IImagenDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ImagenDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ImagenDataModel?> ObtenerPorIdAsync(int id_imagen, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ImagenRepository.ObtenerPorIdAsync(id_imagen, cancellationToken);
            return entity is null ? null : ImagenDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<ImagenDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ImagenRepository.ObtenerPorProductoAsync(id_producto, cancellationToken);
            return entities.Select(ImagenDataMapper.ToDataModel).ToList();
        }

        public async Task<ImagenDataModel> CrearAsync(ImagenDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ImagenDataMapper.ToEntity(model);
            await _unitOfWork.ImagenRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ImagenDataMapper.ToDataModel(entity);
        }

        public async Task<ImagenDataModel?> ActualizarAsync(ImagenDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ImagenRepository.ObtenerParaActualizarAsync(model.id_imagen, cancellationToken);
            if (entity is null) return null;
            ImagenDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ImagenRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ImagenDataMapper.ToDataModel(entity);
        }
    }
}
