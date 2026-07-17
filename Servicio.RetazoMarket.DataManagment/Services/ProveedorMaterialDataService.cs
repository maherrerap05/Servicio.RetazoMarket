using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ProveedorMaterialDataService : IProveedorMaterialDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProveedorMaterialDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ProveedorMaterialDataModel?> ObtenerAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorMaterialRepository.ObtenerAsync(id_material, id_proveedor, cancellationToken);
            return entity is null ? null : ProveedorMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<ProveedorMaterialDataModel>> ObtenerPorProveedorAsync(int id_proveedor, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProveedorMaterialRepository.ObtenerPorProveedorAsync(id_proveedor, cancellationToken);
            return entities.Select(ProveedorMaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<IReadOnlyList<ProveedorMaterialDataModel>> ObtenerPorMaterialAsync(int id_material, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProveedorMaterialRepository.ObtenerPorMaterialAsync(id_material, cancellationToken);
            return entities.Select(ProveedorMaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<ProveedorMaterialDataModel> CrearAsync(ProveedorMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ProveedorMaterialDataMapper.ToEntity(model);
            await _unitOfWork.ProveedorMaterialRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProveedorMaterialDataMapper.ToDataModel(entity);
        }

        public async Task<ProveedorMaterialDataModel?> ActualizarAsync(ProveedorMaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorMaterialRepository.ObtenerParaActualizarAsync(
                model.id_material, model.id_proveedor, cancellationToken);
            if (entity is null) return null;
            ProveedorMaterialDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ProveedorMaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProveedorMaterialDataMapper.ToDataModel(entity);
        }

        public Task<bool> ExisteAsync(int id_material, int id_proveedor, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProveedorMaterialRepository.ExisteAsync(id_material, id_proveedor, cancellationToken);
    }
}
