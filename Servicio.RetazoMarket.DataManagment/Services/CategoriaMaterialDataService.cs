using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class CategoriaMaterialDataService : ICategoriaMaterialDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaMaterialDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<CategoriaMaterialDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.CategoriaMaterialRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(CategoriaMaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<CategoriaMaterialDataModel?> ObtenerPorIdAsync(int id_categoria, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.CategoriaMaterialRepository.ObtenerPorIdAsync(id_categoria, cancellationToken);
            return entity is null ? null : CategoriaMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<CategoriaMaterialDataModel?> ObtenerPorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.CategoriaMaterialRepository.ObtenerPorNombreAsync(cat_nombre, cancellationToken);
            return entity is null ? null : CategoriaMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<CategoriaMaterialDataModel> CrearAsync(CategoriaMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = CategoriaMaterialDataMapper.ToEntity(model);
            await _unitOfWork.CategoriaMaterialRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return CategoriaMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<CategoriaMaterialDataModel?> ActualizarAsync(CategoriaMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.CategoriaMaterialRepository.ObtenerParaActualizarAsync(model.id_categoria, cancellationToken);
            if (entity is null) return null;
            CategoriaMaterialDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.CategoriaMaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return CategoriaMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_categoria, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.CategoriaMaterialRepository.ObtenerParaActualizarAsync(id_categoria, cancellationToken);
            if (entity is null) return false;
            entity.cat_estado = "INA";
            _unitOfWork.CategoriaMaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorNombreAsync(string cat_nombre, CancellationToken cancellationToken = default) =>
            _unitOfWork.CategoriaMaterialRepository.ExistePorNombreAsync(cat_nombre, cancellationToken);
    }
}
