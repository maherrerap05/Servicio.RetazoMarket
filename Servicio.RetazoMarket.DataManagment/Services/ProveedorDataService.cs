using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ProveedorDataService : IProveedorDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProveedorDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<ProveedorDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ProveedorRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(ProveedorDataMapper.ToDataModel).ToList();
        }

        public async Task<ProveedorDataModel?> ObtenerPorIdAsync(int id_proveedor, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorRepository.ObtenerPorIdAsync(id_proveedor, cancellationToken);
            return entity is null ? null : ProveedorDataMapper.ToDataModel(entity);
        }

        public async Task<ProveedorDataModel?> ObtenerPorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorRepository.ObtenerPorCodigoAsync(codigo_proveedor, cancellationToken);
            return entity is null ? null : ProveedorDataMapper.ToDataModel(entity);
        }

        public async Task<ProveedorDataModel?> ObtenerPorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorRepository.ObtenerPorCorreoAsync(prov_correo, cancellationToken);
            return entity is null ? null : ProveedorDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<ProveedorDataModel>> BuscarAsync(ProveedorFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ProveedorQueryRepository.BuscarAsync(filtro.codigo_proveedor, filtro.nombre, filtro.correo, filtro.estado,
                filtro.id_material, filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, ProveedorDataMapper.ToDataModel);
        }

        public async Task<ProveedorDataModel> CrearAsync(ProveedorDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ProveedorDataMapper.ToEntity(model);
            await _unitOfWork.ProveedorRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProveedorDataMapper.ToDataModel(entity);
        }

        public async Task<ProveedorDataModel?> ActualizarAsync(ProveedorDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorRepository.ObtenerParaActualizarAsync(model.id_proveedor, cancellationToken);
            if (entity is null) return null;
            ProveedorDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ProveedorRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ProveedorDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_proveedor, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ProveedorRepository.ObtenerParaActualizarAsync(id_proveedor, cancellationToken);
            if (entity is null) return false;
            entity.prov_estado = "INA";
            _unitOfWork.ProveedorRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorCorreoAsync(string prov_correo, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProveedorRepository.ExistePorCorreoAsync(prov_correo, cancellationToken);

        public Task<bool> ExistePorCodigoAsync(string codigo_proveedor, CancellationToken cancellationToken = default) =>
            _unitOfWork.ProveedorRepository.ExistePorCodigoAsync(codigo_proveedor, cancellationToken);
    }
}
