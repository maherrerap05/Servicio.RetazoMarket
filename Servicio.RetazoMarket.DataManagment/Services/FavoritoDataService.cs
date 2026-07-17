using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;
using Servicio.RetazoMarket.DataManagment.Models.Common;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class FavoritoDataService : IFavoritoDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FavoritoDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<FavoritoDataModel?> ObtenerAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.FavoritoRepository.ObtenerAsync(id_cliente, id_producto, cancellationToken);
            return entity is null ? null : FavoritoDataMapper.ToDataModel(entity);
        }

        public async Task<IReadOnlyList<FavoritoDataModel>> ObtenerPorClienteAsync(int id_cliente, CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.FavoritoRepository.ObtenerPorClienteAsync(id_cliente, cancellationToken);
            return entities.Select(FavoritoDataMapper.ToDataModel).ToList();
        }

        public async Task<DataPagedResult<FavoritoDataModel>> BuscarPorClienteAsync(
            FavoritoFiltroDataModel filtro, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.FavoritoQueryRepository.BuscarPorClienteAsync(
                filtro.id_cliente, filtro.nombre_producto, filtro.id_linea, filtro.estado_producto,
                filtro.PageNumber, filtro.PageSize, cancellationToken);
            return DataPagedResultMapper.ToDataPagedResult(result, FavoritoDataMapper.ToDataModel);
        }

        public async Task<FavoritoDataModel> CrearAsync(FavoritoDataModel model, CancellationToken cancellationToken = default)
        {
            var entity = FavoritoDataMapper.ToEntity(model);
            await _unitOfWork.FavoritoRepository.AgregarAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return FavoritoDataMapper.ToDataModel(entity);
        }

        public async Task<bool> EliminarAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.FavoritoRepository.ObtenerAsync(id_cliente, id_producto, cancellationToken);
            if (entity is null) return false;
            _unitOfWork.FavoritoRepository.Eliminar(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> ExisteAsync(int id_cliente, int id_producto, CancellationToken cancellationToken = default) =>
            _unitOfWork.FavoritoRepository.ExisteAsync(id_cliente, id_producto, cancellationToken);
    }
}
