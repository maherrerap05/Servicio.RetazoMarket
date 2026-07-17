using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class MaterialDataService : IMaterialDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaterialDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<MaterialDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.MaterialRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(MaterialDataMapper.ToDataModel).ToList();
        }

        public async Task<MaterialDataModel?> ObtenerPorIdAsync(int id_material, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MaterialRepository.ObtenerPorIdAsync(id_material, cancellationToken);
            return entity is null ? null : MaterialDataMapper.ToDataModel(entity);
        }

        public async Task<MaterialDataModel?> ObtenerPorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MaterialRepository.ObtenerPorNombreAsync(mat_nombre, cancellationToken);
            return entity is null ? null : MaterialDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<MaterialDataModel>> BuscarAsync(MaterialFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.MaterialQueryRepository.BuscarAsync(filtro.nombre, filtro.id_categoria, filtro.estado,
                filtro.stockMinimo, filtro.stockMaximo, filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, MaterialDataMapper.ToDataModel);
        }

        public async Task<MaterialDataModel> CrearAsync(MaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = MaterialDataMapper.ToEntity(model);
            await _unitOfWork.MaterialRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MaterialDataMapper.ToDataModel(entity);
        }

        public async Task<MaterialDataModel?> ActualizarAsync(MaterialDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MaterialRepository.ObtenerParaActualizarAsync(model.id_material, cancellationToken);
            if (entity is null) return null;
            MaterialDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.MaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MaterialDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_material, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MaterialRepository.ObtenerParaActualizarAsync(id_material, cancellationToken);
            if (entity is null) return false;
            entity.mat_estado = "INA";
            _unitOfWork.MaterialRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorNombreAsync(string mat_nombre, CancellationToken cancellationToken = default) =>
            _unitOfWork.MaterialRepository.ExistePorNombreAsync(mat_nombre, cancellationToken);
    }
}
