using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class UsuarioDataService : IUsuarioDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<UsuarioDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.UsuarioRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(UsuarioDataMapper.ToDataModel).ToList();
        }

        public async Task<IReadOnlyList<UsuarioDataModel>> ObtenerActivosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.UsuarioQueryRepository.ObtenerUsuariosActivosAsync(cancellationToken);
            return entities.Select(UsuarioDataMapper.ToDataModel).ToList();
        }

        public async Task<UsuarioDataModel?> ObtenerPorIdAsync(int id_usuario, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.UsuarioRepository.ObtenerPorIdAsync(id_usuario, cancellationToken);
            return entity is null ? null : UsuarioDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioDataModel?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.UsuarioRepository.ObtenerPorCorreoAsync(correo, cancellationToken);
            return entity is null ? null : UsuarioDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<UsuarioDataModel>> BuscarAsync(UsuarioFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.UsuarioQueryRepository.BuscarAsync(filtro.nombre, filtro.correo, filtro.estado,
                filtro.id_rol, filtro.id_cliente, filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, UsuarioDataMapper.ToDataModel);
        }

        public async Task<UsuarioDataModel> CrearAsync(UsuarioDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = UsuarioDataMapper.ToEntity(model);
            await _unitOfWork.UsuarioRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return UsuarioDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioDataModel?> ActualizarAsync(UsuarioDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.UsuarioRepository.ObtenerParaActualizarAsync(model.id_usuario, cancellationToken);
            if (entity is null) return null;
            UsuarioDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.UsuarioRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return UsuarioDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_usuario, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.UsuarioRepository.ObtenerParaActualizarAsync(id_usuario, cancellationToken);
            if (entity is null) return false;
            entity.usr_estado = "INA";
            _unitOfWork.UsuarioRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default) =>
            _unitOfWork.UsuarioRepository.ExistePorCorreoAsync(correo, cancellationToken);
    }
}
