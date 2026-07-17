using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class MetodoPagoDataService : IMetodoPagoDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MetodoPagoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<MetodoPagoDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.MetodoPagoRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(MetodoPagoDataMapper.ToDataModel).ToList();
        }

        public async Task<MetodoPagoDataModel?> ObtenerPorIdAsync(int id_metodo, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MetodoPagoRepository.ObtenerPorIdAsync(id_metodo, cancellationToken);
            return entity is null ? null : MetodoPagoDataMapper.ToDataModel(entity);
        }

        public async Task<MetodoPagoDataModel?> ObtenerPorNombreAsync(string met_nombre, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MetodoPagoRepository.ObtenerPorNombreAsync(met_nombre, cancellationToken);
            return entity is null ? null : MetodoPagoDataMapper.ToDataModel(entity);
        }

        public async Task<MetodoPagoDataModel?> ObtenerPorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MetodoPagoRepository.ObtenerPorCodigoSriAsync(codigo_sri, cancellationToken);
            return entity is null ? null : MetodoPagoDataMapper.ToDataModel(entity);
        }

        public async Task<MetodoPagoDataModel> CrearAsync(MetodoPagoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = MetodoPagoDataMapper.ToEntity(model);
            await _unitOfWork.MetodoPagoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MetodoPagoDataMapper.ToDataModel(entity);
        }

        public async Task<MetodoPagoDataModel?> ActualizarAsync(MetodoPagoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.MetodoPagoRepository.ObtenerParaActualizarAsync(model.id_metodo, cancellationToken);
            if (entity is null) return null;
            MetodoPagoDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.MetodoPagoRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return MetodoPagoDataMapper.ToDataModel(entity);
        }

        public Task<bool> ExistePorNombreAsync(string met_nombre, CancellationToken cancellationToken = default) =>
            _unitOfWork.MetodoPagoRepository.ExistePorNombreAsync(met_nombre, cancellationToken);

        public Task<bool> ExistePorCodigoSriAsync(string codigo_sri, CancellationToken cancellationToken = default) =>
            _unitOfWork.MetodoPagoRepository.ExistePorCodigoSriAsync(codigo_sri, cancellationToken);
    }
}
