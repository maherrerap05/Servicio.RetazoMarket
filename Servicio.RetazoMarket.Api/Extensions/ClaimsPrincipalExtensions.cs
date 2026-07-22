using System.Security.Claims;
using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.Exceptions;

namespace Servicio.RetazoMarket.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    private const string IdClienteClaim = "id_cliente";

    public static ActorContext ToActorContext(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var autenticado = principal.Identity?.IsAuthenticated == true;

        if (!autenticado)
        {
            return new ActorContext
            {
                correo = string.Empty,
                autenticado = false
            };
        }

        var idUsuarioValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? principal.FindFirst("sub")?.Value;

        if (!int.TryParse(idUsuarioValue, out var idUsuario) || idUsuario <= 0)
            throw new UnauthorizedBusinessException("El token no contiene un identificador de usuario válido.");

        int? idCliente = null;
        var idClienteValue = principal.FindFirst(IdClienteClaim)?.Value;

        if (!string.IsNullOrWhiteSpace(idClienteValue))
        {
            if (!int.TryParse(idClienteValue, out var parsedIdCliente) || parsedIdCliente <= 0)
                throw new UnauthorizedBusinessException("El token contiene un identificador de cliente inválido.");

            idCliente = parsedIdCliente;
        }

        var correo = principal.FindFirst(ClaimTypes.Email)?.Value
            ?? principal.FindFirst("email")?.Value
            ?? string.Empty;

        var roles = principal.Claims
            .Where(claim => claim.Type == ClaimTypes.Role || claim.Type == "role")
            .Select(claim => claim.Value)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new ActorContext
        {
            id_usuario = idUsuario,
            id_cliente = idCliente,
            correo = correo,
            roles = roles,
            autenticado = true
        };
    }
}
