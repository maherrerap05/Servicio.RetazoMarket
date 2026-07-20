using System.Net.Mail;
using Servicio.RetazoMarket.Business.DTOs.Usuario;

namespace Servicio.RetazoMarket.Business.Validators;
public static class UsuarioValidator
{
    public static IReadOnlyCollection<string> ValidarCreacion(CrearUsuarioRequest request)
    {
        if (request is null) return ["La solicitud no puede ser nula."];
        var errors = Validar(request.id_rol, request.nombre, request.correo, request.usr_estado).ToList();
        if (string.IsNullOrWhiteSpace(request.contrasena) || request.contrasena.Length < 8) errors.Add("La contraseña debe tener al menos 8 caracteres.");
        if (request.contrasena?.Length > 128) errors.Add("La contraseña no puede exceder 128 caracteres.");
        return errors;
    }

    public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarUsuarioRequest request)
    {
        if (request is null) return ["La solicitud no puede ser nula."];
        var errors = Validar(request.id_rol, request.nombre, request.correo, request.usr_estado).ToList();
        if (request.id_usuario <= 0) errors.Insert(0, "El id del usuario es inválido.");
        return errors;
    }

    public static IReadOnlyCollection<string> ValidarFiltro(UsuarioFiltroRequest request)
    {
        var errors = new List<string>();
        if (request is null) return ["El filtro no puede ser nulo."];
        if (request.page_number <= 0) errors.Add("El número de página debe ser mayor que cero.");
        if (request.page_size <= 0 || request.page_size > 100) errors.Add("El tamaño de página debe estar entre 1 y 100.");
        return errors;
    }

    private static IReadOnlyCollection<string> Validar(int idRol, string? nombre, string? correo, string? estado)
    {
        var errors = new List<string>();
        if (idRol <= 0) errors.Add("El rol es obligatorio.");
        if (string.IsNullOrWhiteSpace(nombre)) errors.Add("El nombre es obligatorio."); else if (nombre.Trim().Length > 100) errors.Add("El nombre no puede exceder 100 caracteres.");
        if (string.IsNullOrWhiteSpace(correo)) errors.Add("El correo es obligatorio."); else { try { if (new MailAddress(correo.Trim()).Address != correo.Trim() || correo.Trim().Length > 100) errors.Add("El correo no es válido."); } catch { errors.Add("El correo no es válido."); } }
        if (estado?.Trim().ToUpperInvariant() is not ("ACT" or "INA")) errors.Add("El estado debe ser ACT o INA.");
        return errors;
    }
}
