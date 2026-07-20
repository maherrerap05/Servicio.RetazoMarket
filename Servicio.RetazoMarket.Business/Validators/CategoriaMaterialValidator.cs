using Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial;

namespace Servicio.RetazoMarket.Business.Validators
{
    public static class CategoriaMaterialValidator
    {
        public static IReadOnlyCollection<string> ValidarCreacion(CrearCategoriaMaterialRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            return ValidarCampos(request.cat_nombre, request.cat_estado);
        }

        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarCategoriaMaterialRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            var errors = ValidarCampos(request.cat_nombre, request.cat_estado).ToList();
            if (request.id_categoria <= 0)
                errors.Insert(0, "El id de la categoría es inválido.");
            return errors;
        }

        private static IReadOnlyCollection<string> ValidarCampos(string? nombre, string? estado)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(nombre))
                errors.Add("El nombre de la categoría es obligatorio.");
            else if (nombre.Trim().Length > 100)
                errors.Add("El nombre de la categoría no puede exceder 100 caracteres.");

            var estadoNormalizado = estado?.Trim().ToUpperInvariant();
            if (estadoNormalizado is not ("ACT" or "INA"))
                errors.Add("El estado de la categoría debe ser ACT o INA.");
            return errors;
        }
    }
}
