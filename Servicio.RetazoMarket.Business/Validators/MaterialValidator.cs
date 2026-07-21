using Servicio.RetazoMarket.Business.DTOs.Material;

namespace Servicio.RetazoMarket.Business.Validators;

public static class MaterialValidator
{
    public static IReadOnlyCollection<string> Validar(CrearMaterialRequest request)
    {
        if (request is null) return ["La solicitud no puede ser nula."];
        var errores = ValidarCampos(request.id_categoria, request.mat_nombre, request.unidad_medida, request.mat_estado).ToList();
        if (request.stock_actual < 0) errores.Add("El stock no puede ser negativo.");
        if (decimal.Round(request.stock_actual, 3) != request.stock_actual) errores.Add("El stock admite máximo tres decimales.");
        return errores;
    }

    public static IReadOnlyCollection<string> Validar(ActualizarMaterialRequest request)
    {
        if (request is null) return ["La solicitud no puede ser nula."];
        var errores = ValidarCampos(request.id_categoria, request.mat_nombre, request.unidad_medida, request.mat_estado).ToList();
        if (request.id_material <= 0) errores.Insert(0, "El id del material es inválido.");
        return errores;
    }

    public static IReadOnlyCollection<string> ValidarFiltro(MaterialFiltroRequest request)
    {
        if (request is null) return ["El filtro no puede ser nulo."];
        var errores = new List<string>();
        if (request.codigo_proveedor?.Trim().Length > 200) errores.Add("El código del proveedor no puede exceder 200 caracteres.");
        if (request.page_number <= 0) errores.Add("El número de página debe ser mayor que cero.");
        if (request.page_size <= 0 || request.page_size > 100) errores.Add("El tamaño de página debe estar entre 1 y 100.");
        if (request.stock_minimo < 0 || request.stock_maximo < 0) errores.Add("Los límites de stock no pueden ser negativos.");
        if (request.stock_minimo > request.stock_maximo) errores.Add("El stock mínimo no puede superar al máximo.");
        return errores;
    }

    private static IReadOnlyCollection<string> ValidarCampos(int categoria, string? nombre, string? unidad, string? estado)
    {
        var errores = new List<string>();
        if (categoria <= 0) errores.Add("La categoría es obligatoria.");
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 100) errores.Add("El nombre es obligatorio y admite hasta 100 caracteres.");
        if (string.IsNullOrWhiteSpace(unidad) || unidad.Trim().Length > 50) errores.Add("La unidad de medida es obligatoria y admite hasta 50 caracteres.");
        if (estado?.Trim().ToUpperInvariant() is not ("ACT" or "INA")) errores.Add("El estado debe ser ACT o INA.");
        return errores;
    }
}
