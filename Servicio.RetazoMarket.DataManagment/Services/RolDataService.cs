using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class RolDataService : IRolDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<RolDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.RolRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(RolDataMapper.ToDataModel).ToList();
        }

        public async Task<RolDataModel?> ObtenerPorIdAsync(int id_rol, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.RolRepository.ObtenerPorIdAsync(id_rol, cancellationToken);
            return entity is null ? null : RolDataMapper.ToDataModel(entity);
        }

        public async Task<RolDataModel?> ObtenerPorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.RolRepository.ObtenerPorNombreAsync(nombre_rol, cancellationToken);
            return entity is null ? null : RolDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<RolDataModel>> BuscarAsync(string? nombre_rol, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.RolQueryRepository.BuscarAsync(nombre_rol, pageNumber, pageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, RolDataMapper.ToDataModel);
        }

        public async Task<RolDataModel> CrearAsync(RolDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = RolDataMapper.ToEntity(model);
            await _unitOfWork.RolRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RolDataMapper.ToDataModel(entity);
        }

        public async Task<RolDataModel?> ActualizarAsync(RolDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.RolRepository.ObtenerParaActualizarAsync(model.id_rol, cancellationToken);
            if (entity is null) return null;
            RolDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.RolRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RolDataMapper.ToDataModel(entity);
        }

        public Task<bool> ExistePorNombreAsync(string nombre_rol, CancellationToken cancellationToken = default) =>
            _unitOfWork.RolRepository.ExistePorNombreAsync(nombre_rol, cancellationToken);
    }
}
