using Servicio.RetazoMarket.Business.DTOs.Rol;

namespace Servicio.RetazoMarket.Business.Validators
{
    public static class RolValidator
    {
        private static readonly HashSet<string> RolesPermitidos =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "SUPERADMINISTRADOR", "ADMINISTRADOR", "CLIENTE"
            };

        public static IReadOnlyCollection<string> ValidarCreacion(CrearRolRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            return ValidarNombre(request.nombre_rol);
        }

        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarRolRequest request)
        {
            if (request is null) return ["La solicitud no puede ser nula."];
            var errors = ValidarNombre(request.nombre_rol).ToList();
            if (request.id_rol <= 0) errors.Insert(0, "El id del rol es inválido.");
            return errors;
        }

        public static IReadOnlyCollection<string> ValidarFiltro(RolFiltroRequest request)
        {
            var errors = new List<string>();
            if (request is null) return ["La solicitud de filtro no puede ser nula."];
            if (request.nombre_rol?.Trim().Length > 100)
                errors.Add("El nombre del rol no puede exceder 100 caracteres.");
            if (request.page_number <= 0)
                errors.Add("El número de página debe ser mayor que cero.");
            if (request.page_size <= 0 || request.page_size > 100)
                errors.Add("El tamaño de página debe estar entre 1 y 100.");
            return errors;
        }

        private static IReadOnlyCollection<string> ValidarNombre(string? nombre)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(nombre))
                errors.Add("El nombre del rol es obligatorio.");
            else
            {
                var normalizado = nombre.Trim();
                if (normalizado.Length > 100)
                    errors.Add("El nombre del rol no puede exceder 100 caracteres.");
                if (!RolesPermitidos.Contains(normalizado))
                    errors.Add("El rol debe ser SUPERADMINISTRADOR, ADMINISTRADOR o CLIENTE.");
            }
            return errors;
        }
    }
}
