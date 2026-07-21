using System.Net.Mail;
using Servicio.RetazoMarket.Business.DTOs.Proveedor;

namespace Servicio.RetazoMarket.Business.Validators;

public static class ProveedorValidator
{
    public static IReadOnlyCollection<string> Validar(CrearProveedorRequest request) =>
        request is null
            ? ["La solicitud no puede ser nula."]
            : ValidarCampos(request.codigo_proveedor, request.prov_nombre, request.prov_telefono,
                request.prov_correo, request.prov_direccion, request.prov_estado);

    public static IReadOnlyCollection<string> Validar(ActualizarProveedorRequest request)
    {
        if (request is null)
            return ["La solicitud no puede ser nula."];

        var errores = ValidarCampos(request.codigo_proveedor, request.prov_nombre, request.prov_telefono,
            request.prov_correo, request.prov_direccion, request.prov_estado).ToList();

        if (request.id_proveedor <= 0)
            errores.Insert(0, "El id del proveedor es inválido.");

        return errores;
    }

    public static IReadOnlyCollection<string> ValidarFiltro(ProveedorFiltroRequest request)
    {
        if (request is null)
            return ["El filtro no puede ser nulo."];

        var errores = new List<string>();
        if (request.codigo_proveedor?.Trim().Length > 200)
            errores.Add("El código del proveedor no puede exceder 200 caracteres.");
        if (request.page_number <= 0)
            errores.Add("El número de página debe ser mayor que cero.");
        if (request.page_size <= 0 || request.page_size > 100)
            errores.Add("El tamaño de página debe estar entre 1 y 100.");

        return errores;
    }

    private static IReadOnlyCollection<string> ValidarCampos(
        string? codigo, string? nombre, string? telefono, string? correo, string? direccion, string? estado)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(codigo) || codigo.Trim().Length > 200)
            errores.Add("El código del proveedor es obligatorio y admite hasta 200 caracteres.");
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 100)
            errores.Add("El nombre es obligatorio y admite hasta 100 caracteres.");
        if (string.IsNullOrWhiteSpace(telefono) || telefono.Trim().Length != 10 || !telefono.Trim().All(char.IsDigit))
            errores.Add("El teléfono debe contener exactamente 10 dígitos.");
        if (string.IsNullOrWhiteSpace(correo) || correo.Trim().Length > 100 || !EsCorreoValido(correo))
            errores.Add("El correo no es válido.");
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Trim().Length > 255)
            errores.Add("La dirección es obligatoria y admite hasta 255 caracteres.");
        if (estado?.Trim().ToUpperInvariant() is not ("ACT" or "INA"))
            errores.Add("El estado debe ser ACT o INA.");

        return errores;
    }

    private static bool EsCorreoValido(string correo)
    {
        try
        {
            return new MailAddress(correo.Trim()).Address == correo.Trim();
        }
        catch
        {
            return false;
        }
    }
}
