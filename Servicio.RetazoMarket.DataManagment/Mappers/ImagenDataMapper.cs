using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class ImagenDataMapper
    {
        public static ImagenDataModel ToDataModel(ImagenEntity entity) => new()
        {
            id_imagen = entity.id_imagen,
            id_producto = entity.id_producto,
            url = entity.url,
            es_principal = entity.es_principal,
            orden = entity.orden
        };

        public static ImagenEntity ToEntity(ImagenDataModel model) => new()
        {
            id_imagen = model.id_imagen,
            id_producto = model.id_producto,
            url = model.url,
            es_principal = model.es_principal,
            orden = model.orden
        };

        public static void ApplyToEntity(ImagenDataModel model, ImagenEntity entity)
        {
            entity.id_producto = model.id_producto;
            entity.url = model.url;
            entity.es_principal = model.es_principal;
            entity.orden = model.orden;
        }
    }
}
