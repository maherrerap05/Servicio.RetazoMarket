using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Validators;
using Servicio.RetazoMarket.DataManagment.Interfaces;

namespace Servicio.RetazoMarket.Business.Services;
public class AuthService : IAuthService
{
    private readonly IUsuarioDataService _usuarios;
    private readonly IClienteDataService _clientes;
    private readonly IPasswordHashService _passwords;
    public AuthService(IUsuarioDataService usuarios, IClienteDataService clientes, IPasswordHashService passwords) => (_usuarios, _clientes, _passwords) = (usuarios, clientes, passwords);

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var errors = AuthValidator.ValidarLogin(request);
        if (errors.Any()) throw new ValidationException("Solicitud de login inválida.", errors);
        var usuario = await _usuarios.ObtenerPorCorreoAsync(request.UserName.Trim().ToLowerInvariant(), cancellationToken);
        if (usuario is null || !_passwords.Verify(request.Password, usuario.contrasena_hash)) throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");
        if (usuario.usr_estado != "ACT") throw new UnauthorizedBusinessException("El usuario se encuentra inactivo.");
        if (usuario.id_cliente.HasValue)
        {
            var cliente = await _clientes.ObtenerPorIdAsync(usuario.id_cliente.Value, cancellationToken);
            if (cliente is null || cliente.cli_estado != "ACT") throw new UnauthorizedBusinessException("El cliente asociado se encuentra inactivo.");
        }
        return new LoginResponse { UserName=usuario.nombre, Correo=usuario.correo, Activo=true, IdCliente=usuario.id_cliente is > 0 ? usuario.id_cliente : null, Roles=usuario.Rol is null ? [] : [usuario.Rol.nombre_rol], Token=string.Empty, ExpirationUtc=DateTime.MinValue };
    }
}
