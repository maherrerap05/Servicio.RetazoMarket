using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class PersonalizacionDataService : IPersonalizacionDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonalizacionDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<PersonalizacionDataModel?> ObtenerPorIdAsync(int id_opcion, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.PersonalizacionRepository.ObtenerPorIdAsync(id_opcion, cancellationToken);
            return entity is null ? null : PersonalizacionDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<PersonalizacionDataModel>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.PersonalizacionRepository.ObtenerPorProductoAsync(id_producto, cancellationToken);
            return entities.Select(PersonalizacionDataMapper.ToDataModel).ToList();
        }

        public async Task<PersonalizacionDataModel> CrearAsync(PersonalizacionDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = PersonalizacionDataMapper.ToEntity(model);
            await _unitOfWork.PersonalizacionRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return PersonalizacionDataMapper.ToDataModel(entity);
        }

        public async Task<PersonalizacionDataModel?> ActualizarAsync(PersonalizacionDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.PersonalizacionRepository.ObtenerParaActualizarAsync(model.id_opcion, cancellationToken);
            if (entity is null) return null;
            PersonalizacionDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.PersonalizacionRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return PersonalizacionDataMapper.ToDataModel(entity);
        }
    }
}
