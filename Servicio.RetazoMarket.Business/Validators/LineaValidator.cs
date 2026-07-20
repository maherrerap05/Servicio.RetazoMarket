using Servicio.RetazoMarket.Business.DTOs.Linea;

namespace Servicio.RetazoMarket.Business.Validators
{
    public static class LineaValidator
    {
        public static IReadOnlyCollection<string> ValidarCreacion(CrearLineaRequest request)
        {
            var errors = new List<string>();

            if (request == null) { errors.Add("La solicitud no puede ser nula."); return errors; }

            if (string.IsNullOrWhiteSpace(request.lin_nombre))
                errors.Add("El nombre de la línea es obligatorio.");
            if (request.lin_nombre != null && request.lin_nombre.Trim().Length > 100)
                errors.Add("El nombre de la línea no puede exceder 100 caracteres.");

            if (string.IsNullOrWhiteSpace(request.lin_estado))
                errors.Add("El estado de la línea es obligatorio.");
            if (!string.IsNullOrWhiteSpace(request.lin_estado) &&
                request.lin_estado != "ACT" && request.lin_estado != "INA")
                errors.Add("El estado de la línea debe ser ACT o INA.");

            return errors;
        }

        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarLineaRequest request)
        {
            var errors = new List<string>();

            if (request == null) { errors.Add("La solicitud no puede ser nula."); return errors; }

            if (request.id_linea <= 0)
                errors.Add("El id de la línea es inválido.");

            if (string.IsNullOrWhiteSpace(request.lin_nombre))
                errors.Add("El nombre de la línea es obligatorio.");
            if (request.lin_nombre != null && request.lin_nombre.Trim().Length > 100)
                errors.Add("El nombre de la línea no puede exceder 100 caracteres.");

            if (string.IsNullOrWhiteSpace(request.lin_estado))
                errors.Add("El estado de la línea es obligatorio.");
            if (!string.IsNullOrWhiteSpace(request.lin_estado) &&
                request.lin_estado != "ACT" && request.lin_estado != "INA")
                errors.Add("El estado de la línea debe ser ACT o INA.");

            return errors;
        }

        public static IReadOnlyCollection<string> ValidarFiltro(LineaFiltroRequest request)
        {
            var errors = new List<string>();

            if (request == null) { errors.Add("La solicitud de filtro no puede ser nula."); return errors; }

            if (!string.IsNullOrWhiteSpace(request.lin_nombre) && request.lin_nombre.Trim().Length > 100)
                errors.Add("El nombre de la línea no puede exceder 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(request.lin_estado) &&
                request.lin_estado != "ACT" && request.lin_estado != "INA")
                errors.Add("El estado del filtro debe ser ACT o INA.");

            if (request.page_number <= 0)
                errors.Add("El número de página debe ser mayor que cero.");
            if (request.page_size <= 0)
                errors.Add("El tamaño de página debe ser mayor que cero.");
            if (request.page_size > 100)
                errors.Add("El tamaño de página no puede ser mayor a 100.");

            return errors;
        }
    }
}
