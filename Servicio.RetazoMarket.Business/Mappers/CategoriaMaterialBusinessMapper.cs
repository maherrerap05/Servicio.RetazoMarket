using Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers
{
    public static class CategoriaMaterialBusinessMapper
    {
        public static CategoriaMaterialDataModel ToDataModel(CrearCategoriaMaterialRequest request) => new()
        {
            cat_nombre = request.cat_nombre.Trim(),
            cat_estado = request.cat_estado.Trim().ToUpperInvariant()
        };

        public static CategoriaMaterialDataModel ToDataModel(ActualizarCategoriaMaterialRequest request) => new()
        {
            id_categoria = request.id_categoria,
            cat_nombre = request.cat_nombre.Trim(),
            cat_estado = request.cat_estado.Trim().ToUpperInvariant()
        };

        public static CategoriaMaterialResponse ToResponse(CategoriaMaterialDataModel model) => new()
        {
            id_categoria = model.id_categoria,
            cat_nombre = model.cat_nombre,
            cat_estado = model.cat_estado
        };
    }
}
