using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class MetodoPagoDataMapper
    {
        public static MetodoPagoDataModel ToDataModel(MetodoPagoEntity entity) => new()
        {
            id_metodo = entity.id_metodo,
            met_nombre = entity.met_nombre,
            codigo_sri = entity.codigo_sri
        };

        public static MetodoPagoEntity ToEntity(MetodoPagoDataModel model) => new()
        {
            id_metodo = model.id_metodo,
            met_nombre = model.met_nombre,
            codigo_sri = model.codigo_sri
        };

        public static void ApplyToEntity(MetodoPagoDataModel model, MetodoPagoEntity entity)
        {
            entity.met_nombre = model.met_nombre;
            entity.codigo_sri = model.codigo_sri;
        }
    }
}
