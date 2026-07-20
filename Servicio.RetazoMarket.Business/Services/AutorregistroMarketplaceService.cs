using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Mappers;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;

namespace Servicio.RetazoMarket.Business.Services
{
    public class AutorregistroMarketplaceService : IAutorregistroMarketplaceService
    {
        private readonly IAutorregistroMarketplaceDataService _autorregistroDataService;
        private readonly IClienteDataService _clienteDataService;
        private readonly IUsuarioDataService _usuarioDataService;
        private readonly IRolDataService _rolDataService;
        private readonly IPasswordHashService _passwordHashService;

        public AutorregistroMarketplaceService(
            IAutorregistroMarketplaceDataService autorregistroDataService,
            IClienteDataService clienteDataService,
            IUsuarioDataService usuarioDataService,
            IRolDataService rolDataService,
            IPasswordHashService passwordHashService)
        {
            _autorregistroDataService = autorregistroDataService;
            _clienteDataService = clienteDataService;
            _usuarioDataService = usuarioDataService;
            _rolDataService = rolDataService;
            _passwordHashService = passwordHashService;
        }

        public async Task<AutorregistroMarketplaceResponse> RegistrarAsync(
            AutorregistroMarketplaceRequest request,
            CancellationToken cancellationToken = default)
        {
            var errors = AutorregistroMarketplaceValidator.Validar(request);
            if (errors.Any())
                throw new ValidationException("Solicitud de autorregistro inválida.", errors);

            var correo = request.correo.Trim().ToLowerInvariant();
            if (await _clienteDataService.ExistePorCorreoAsync(correo, cancellationToken) ||
                await _usuarioDataService.ExistePorCorreoAsync(correo, cancellationToken))
                throw new ValidationException("El correo ya pertenece a otra persona.");

            var rolCliente = await _rolDataService.ObtenerPorNombreAsync("CLIENTE", cancellationToken)
                ?? throw new ValidationException("No se encuentra configurado el rol CLIENTE.");

            var model = AutorregistroMarketplaceBusinessMapper.ToDataModel(
                request,
                rolCliente.id_rol,
                _passwordHashService.Hash(request.contrasena));

            try
            {
                var result = await _autorregistroDataService.RegistrarAsync(model, cancellationToken);
                result.Usuario.Rol = new Servicio.RetazoMarket.DataManagment.Models.RolResumenDataModel
                {
                    id_rol = rolCliente.id_rol,
                    nombre_rol = rolCliente.nombre_rol
                };
                return AutorregistroMarketplaceBusinessMapper.ToResponse(result);
            }
            catch (InvalidOperationException exception)
            {
                throw new ValidationException(exception.Message);
            }
        }
    }
}
