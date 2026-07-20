using Servicio.RetazoMarket.Business.DTOs.Auth;

namespace Servicio.RetazoMarket.Business.Validators;
public static class AuthValidator
{
    public static IReadOnlyCollection<string> ValidarLogin(LoginRequest request)
    {
        var errors = new List<string>();
        if (request is null) return ["La solicitud de login no puede ser nula."];
        if (string.IsNullOrWhiteSpace(request.UserName)) errors.Add("El nombre de usuario es obligatorio.");
        else if (request.UserName.Trim().Length > 100) errors.Add("El nombre de usuario no puede exceder 100 caracteres.");
        if (string.IsNullOrWhiteSpace(request.Password)) errors.Add("La contraseña es obligatoria.");
        else if (request.Password.Length > 128) errors.Add("La contraseña no puede exceder 128 caracteres.");
        return errors;
    }
}
