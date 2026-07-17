using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class LineaDataService : ILineaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LineaDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<LineaDataModel>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.LineaRepository.ObtenerTodosAsync(cancellationToken);
            return entities.Select(LineaDataMapper.ToDataModel).ToList();
        }

        public async Task<LineaDataModel?> ObtenerPorIdAsync(int id_linea, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.LineaRepository.ObtenerPorIdAsync(id_linea, cancellationToken);
            return entity is null ? null : LineaDataMapper.ToDataModel(entity);
        }

        public async Task<LineaDataModel?> ObtenerPorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.LineaRepository.ObtenerPorNombreAsync(lin_nombre, cancellationToken);
            return entity is null ? null : LineaDataMapper.ToDataModel(entity);
        }

        public async Task<LineaDataModel> CrearAsync(LineaDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = LineaDataMapper.ToEntity(model);
            await _unitOfWork.LineaRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return LineaDataMapper.ToDataModel(entity);
        }

        public async Task<LineaDataModel?> ActualizarAsync(LineaDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.LineaRepository.ObtenerParaActualizarAsync(model.id_linea, cancellationToken);
            if (entity is null) return null;
            LineaDataMapper.ApplyToEntity(model, entity);
            _unitOfWork.LineaRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return LineaDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarLogicoAsync(int id_linea, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.LineaRepository.ObtenerParaActualizarAsync(id_linea, cancellationToken);
            if (entity is null) return false;
            entity.lin_estado = "INA";
            _unitOfWork.LineaRepository.Actualizar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExistePorNombreAsync(string lin_nombre, CancellationToken cancellationToken = default) =>
            _unitOfWork.LineaRepository.ExistePorNombreAsync(lin_nombre, cancellationToken);
    }
}
