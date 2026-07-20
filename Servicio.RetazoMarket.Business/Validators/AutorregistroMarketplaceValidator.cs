using System.Net.Mail;
using Servicio.RetazoMarket.Business.DTOs.Auth;

namespace Servicio.RetazoMarket.Business.Validators
{
    public static class AutorregistroMarketplaceValidator
    {
        public static IReadOnlyCollection<string> Validar(AutorregistroMarketplaceRequest request)
        {
            var errors = new List<string>();
            if (request is null)
                return ["La solicitud de autorregistro no puede ser nula."];

            if (string.IsNullOrWhiteSpace(request.nombre))
                errors.Add("El nombre es obligatorio.");
            else if (request.nombre.Trim().Length > 100)
                errors.Add("El nombre no puede exceder 100 caracteres.");

            if (request.apellidos?.Trim().Length > 100)
                errors.Add("Los apellidos no pueden exceder 100 caracteres.");

            if (string.IsNullOrWhiteSpace(request.correo) ||
                request.correo.Trim().Length > 100 ||
                !EsCorreoValido(request.correo))
                errors.Add("El correo no es válido.");

            if (string.IsNullOrWhiteSpace(request.telefono) ||
                request.telefono.Trim().Length != 10 ||
                !request.telefono.Trim().All(char.IsDigit))
                errors.Add("El teléfono debe contener exactamente 10 dígitos.");

            if (string.IsNullOrWhiteSpace(request.direccion))
                errors.Add("La dirección es obligatoria.");
            else if (request.direccion.Trim().Length > 255)
                errors.Add("La dirección no puede exceder 255 caracteres.");

            if (string.IsNullOrWhiteSpace(request.contrasena) || request.contrasena.Length < 8)
                errors.Add("La contraseña debe tener al menos 8 caracteres.");
            else if (request.contrasena.Length > 128)
                errors.Add("La contraseña no puede exceder 128 caracteres.");

            return errors;
        }

        private static bool EsCorreoValido(string correo)
        {
            try { return new MailAddress(correo.Trim()).Address == correo.Trim(); }
            catch { return false; }
        }
    }
}
