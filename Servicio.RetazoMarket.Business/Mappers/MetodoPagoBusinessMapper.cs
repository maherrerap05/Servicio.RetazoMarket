using Servicio.RetazoMarket.Business.DTOs.MetodoPago;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers
{
    public static class MetodoPagoBusinessMapper
    {
        public static MetodoPagoDataModel ToDataModel(CrearMetodoPagoRequest request) => new()
        {
            met_nombre = request.met_nombre.Trim(),
            codigo_sri = request.codigo_sri.Trim()
        };

        public static MetodoPagoDataModel ToDataModel(ActualizarMetodoPagoRequest request) => new()
        {
            id_metodo = request.id_metodo,
            met_nombre = request.met_nombre.Trim(),
            codigo_sri = request.codigo_sri.Trim()
        };

        public static MetodoPagoResponse ToResponse(MetodoPagoDataModel model) => new()
        {
            id_metodo = model.id_metodo,
            met_nombre = model.met_nombre,
            codigo_sri = model.codigo_sri
        };
    }
}
