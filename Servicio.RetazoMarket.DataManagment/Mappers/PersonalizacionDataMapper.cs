using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class PersonalizacionDataMapper
    {
        public static PersonalizacionDataModel ToDataModel(PersonalizacionEntity entity) => new()
        {
            id_opcion = entity.id_opcion,
            id_producto = entity.id_producto,
            nombre_atr = entity.nombre_atr,
            tipo_valor = entity.tipo_valor.Trim(),
            valores_json = JsonDataMapper.Clone(entity.valores_json),
            costo_adicional = entity.costo_adicional
        };

        public static PersonalizacionEntity ToEntity(PersonalizacionDataModel model) => new()
        {
            id_opcion = model.id_opcion,
            id_producto = model.id_producto,
            nombre_atr = model.nombre_atr,
            tipo_valor = model.tipo_valor,
            valores_json = JsonDataMapper.ToDocument(model.valores_json),
            costo_adicional = model.costo_adicional
        };

        public static void ApplyToEntity(PersonalizacionDataModel model, PersonalizacionEntity entity)
        {
            entity.id_producto = model.id_producto;
            entity.nombre_atr = model.nombre_atr;
            entity.tipo_valor = model.tipo_valor;
            entity.valores_json = JsonDataMapper.ToDocument(model.valores_json);
            entity.costo_adicional = model.costo_adicional;
        }
    }
}
