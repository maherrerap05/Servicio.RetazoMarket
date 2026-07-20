using System.Data;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services
{
    public class AutorregistroMarketplaceDataService : IAutorregistroMarketplaceDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutorregistroMarketplaceDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AutorregistroMarketplaceResultDataModel> RegistrarAsync(
            AutorregistroMarketplaceDataModel model,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(model.Cliente);
            ArgumentNullException.ThrowIfNull(model.Usuario);

            await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            try
            {
                var correo = model.Cliente.correo;
                if (await _unitOfWork.ClienteRepository.ExistePorCorreoAsync(correo, cancellationToken) ||
                    await _unitOfWork.UsuarioRepository.ExistePorCorreoAsync(correo, cancellationToken))
                {
                    throw new InvalidOperationException("El correo ya se encuentra registrado.");
                }

                var clienteEntity = ClienteDataMapper.ToEntity(model.Cliente);
                await _unitOfWork.ClienteRepository.AgregarAsync(clienteEntity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                model.Usuario.id_cliente = clienteEntity.id_cliente;
                var usuarioEntity = UsuarioDataMapper.ToEntity(model.Usuario);
                await _unitOfWork.UsuarioRepository.AgregarAsync(usuarioEntity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return new AutorregistroMarketplaceResultDataModel
                {
                    Cliente = ClienteDataMapper.ToDataModel(clienteEntity),
                    Usuario = UsuarioDataMapper.ToDataModel(usuarioEntity)
                };
            }
            catch
            {
                try
                {
                    await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                }
                finally
                {
                    _unitOfWork.DescartarCambios();
                }

                throw;
            }
        }
    }
}
