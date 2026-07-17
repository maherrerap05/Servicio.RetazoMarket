using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Mappers
{
    public static class FavoritoDataMapper
    {
        public static FavoritoDataModel ToDataModel(FavoritoEntity entity) => new()
        {
            id_cliente = entity.id_cliente,
            id_producto = entity.id_producto,
            Producto = entity.Producto is null ? null : new FavoritoProductoResumenDataModel
            {
                id_producto = entity.Producto.id_producto,
                id_linea = entity.Producto.id_linea,
                prod_nombre = entity.Producto.prod_nombre,
                prod_descripcion = entity.Producto.prod_descripcion,
                colores = JsonDataMapper.Clone(entity.Producto.colores),
                precio_base = entity.Producto.precio_base,
                es_personalizable = entity.Producto.es_personalizable,
                stock_actual = entity.Producto.stock_actual,
                prod_estado = entity.Producto.prod_estado,
                Linea = entity.Producto.Linea is null ? null : new LineaFavoritoResumenDataModel
                {
                    id_linea = entity.Producto.Linea.id_linea,
                    lin_nombre = entity.Producto.Linea.lin_nombre
                },
                Imagenes = entity.Producto.Imagenes.OrderBy(i => i.orden).ThenBy(i => i.id_imagen)
                    .Select(i => new ImagenFavoritoResumenDataModel
                    {
                        id_imagen = i.id_imagen,
                        url = i.url,
                        es_principal = i.es_principal,
                        orden = i.orden
                    }).ToList()
            }
        };

        public static FavoritoEntity ToEntity(FavoritoDataModel model) => new()
        {
            id_cliente = model.id_cliente,
            id_producto = model.id_producto
        };
    }
}
