using Servicio.RetazoMarket.Business.DTOs.Linea;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers
{
    public static class LineaBusinessMapper
    {
        public static LineaDataModel ToDataModel(CrearLineaRequest request)
        {
            return new LineaDataModel
            {
                lin_nombre = request.lin_nombre.Trim(),
                lin_estado = request.lin_estado.Trim().ToUpperInvariant()
            };
        }

        public static LineaDataModel ToDataModel(ActualizarLineaRequest request)
        {
            return new LineaDataModel
            {
                id_linea = request.id_linea,
                lin_nombre = request.lin_nombre.Trim(),
                lin_estado = request.lin_estado.Trim().ToUpperInvariant()
            };
        }

        public static LineaResponse ToResponse(LineaDataModel model)
        {
            return new LineaResponse
            {
                id_linea = model.id_linea,
                lin_nombre = model.lin_nombre,
                lin_estado = model.lin_estado
            };
        }
    }
}
