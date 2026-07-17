using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class ClienteDataService : IClienteDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<ClienteDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ClienteRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(ClienteDataMapper.ToDataModel).ToList();
        }

        public async Task<IReadOnlyList<ClienteDataModel>> ObtenerActivosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.ClienteQueryRepository.ObtenerClientesActivosAsync(cancellationToken);
            return entities.Select(ClienteDataMapper.ToDataModel).ToList();
        }

        public async Task<ClienteDataModel?> ObtenerPorIdAsync(int id_cliente, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ClienteRepository.ObtenerPorIdAsync(id_cliente, cancellationToken);
            return entity is null ? null : ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<ClienteDataModel?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ClienteRepository.ObtenerPorCorreoAsync(correo, cancellationToken);
            return entity is null ? null : ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<ClienteDataModel>> BuscarAsync(ClienteFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ClienteQueryRepository.BuscarAsync(filtro.nombre, filtro.apellidos, filtro.correo,
                filtro.telefono, filtro.origen, filtro.estado, filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, ClienteDataMapper.ToDataModel);
        }

        public async Task<ClienteDataModel> CrearAsync(ClienteDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = ClienteDataMapper.ToEntity(model);
            await _unitOfWork.ClienteRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<ClienteDataModel?> ActualizarAsync(ClienteDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ClienteRepository.ObtenerParaActualizarAsync(model.id_cliente, cancellationToken);
            if (entity is null) return null;
            ClienteDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.ClienteRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_cliente, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.ClienteRepository.ObtenerParaActualizarAsync(id_cliente, cancellationToken);
            if (entity is null) return false;
            entity.cli_estado = "INA";
            _unitOfWork.ClienteRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default) =>
            _unitOfWork.ClienteRepository.ExistePorCorreoAsync(correo, cancellationToken);
    }
}
