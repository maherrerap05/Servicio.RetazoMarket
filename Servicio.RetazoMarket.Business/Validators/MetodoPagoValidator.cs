using Servicio.RetazoMarket.Business.DTOs.MetodoPago;

namespace Servicio.RetazoMarket.Business.Validators
{
    public static class MetodoPagoValidator
    {
        public static IReadOnlyCollection<string> ValidarCreacion(CrearMetodoPagoRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            return ValidarCampos(request.met_nombre, request.codigo_sri);
        }

        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarMetodoPagoRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            var errors = ValidarCampos(request.met_nombre, request.codigo_sri).ToList();
            if (request.id_metodo <= 0)
                errors.Insert(0, "El id del método de pago es inválido.");
            return errors;
        }

        private static IReadOnlyCollection<string> ValidarCampos(string? nombre, string? codigoSri)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(nombre))
                errors.Add("El nombre del método de pago es obligatorio.");
            else if (nombre.Trim().Length > 100)
                errors.Add("El nombre del método de pago no puede exceder 100 caracteres.");

            if (string.IsNullOrWhiteSpace(codigoSri))
                errors.Add("El código SRI es obligatorio.");
            else if (codigoSri.Trim().Length > 500)
                errors.Add("El código SRI no puede exceder 500 caracteres.");
            return errors;
        }
    }
}
