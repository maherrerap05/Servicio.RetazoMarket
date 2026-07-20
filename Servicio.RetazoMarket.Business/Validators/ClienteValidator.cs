using System.Net.Mail;
using Servicio.RetazoMarket.Business.DTOs.Cliente;

namespace Servicio.RetazoMarket.Business.Validators;
public static class ClienteValidator
{
    public static IReadOnlyCollection<string> ValidarCreacion(CrearClienteRequest request) =>
        request is null ? ["La solicitud no puede ser nula."] : Validar(request.nombre, request.apellidos, request.correo, request.telefono, request.direccion, request.origen, request.cli_estado);

    public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarClienteRequest request)
    {
        if (request is null) return ["La solicitud no puede ser nula."];
        var errors = Validar(request.nombre, request.apellidos, request.correo, request.telefono, request.direccion, request.origen, request.cli_estado).ToList();
        if (request.id_cliente <= 0) errors.Insert(0, "El id del cliente es inválido.");
        return errors;
    }

    public static IReadOnlyCollection<string> ValidarFiltro(ClienteFiltroRequest request)
    {
        var errors = new List<string>();
        if (request is null) return ["El filtro no puede ser nulo."];
        if (request.page_number <= 0) errors.Add("El número de página debe ser mayor que cero.");
        if (request.page_size <= 0 || request.page_size > 100) errors.Add("El tamaño de página debe estar entre 1 y 100.");
        if (request.nombre?.Trim().Length > 100 || request.apellidos?.Trim().Length > 100 || request.correo?.Trim().Length > 100)
            errors.Add("Los filtros de nombre, apellidos y correo no pueden exceder 100 caracteres.");
        return errors;
    }

    private static IReadOnlyCollection<string> Validar(string? nombre, string? apellidos, string? correo, string? telefono, string? direccion, string? origen, string? estado)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(nombre)) errors.Add("El nombre es obligatorio."); else if (nombre.Trim().Length > 100) errors.Add("El nombre no puede exceder 100 caracteres.");
        if (apellidos?.Trim().Length > 100) errors.Add("Los apellidos no pueden exceder 100 caracteres.");
        if (string.IsNullOrWhiteSpace(correo)) errors.Add("El correo es obligatorio."); else if (correo.Trim().Length > 100 || !EsCorreoValido(correo)) errors.Add("El correo no es válido.");
        if (string.IsNullOrWhiteSpace(telefono) || telefono.Trim().Length != 10 || !telefono.Trim().All(char.IsDigit)) errors.Add("El teléfono debe contener exactamente 10 dígitos.");
        if (string.IsNullOrWhiteSpace(direccion)) errors.Add("La dirección es obligatoria."); else if (direccion.Trim().Length > 255) errors.Add("La dirección no puede exceder 255 caracteres.");
        if (origen?.Trim().ToUpperInvariant() is not ("MKT" or "FIS")) errors.Add("El origen debe ser MKT o FIS.");
        if (estado?.Trim().ToUpperInvariant() is not ("ACT" or "INA")) errors.Add("El estado debe ser ACT o INA.");
        return errors;
    }

    private static bool EsCorreoValido(string correo) { try { return new MailAddress(correo.Trim()).Address == correo.Trim(); } catch { return false; } }
}
