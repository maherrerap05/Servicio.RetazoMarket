namespace Servicio.RetazoMarket.Business.DTOs.Linea
{
    public class CrearLineaRequest
    {
        public string lin_nombre { get; set; } = null!;
        public string lin_estado { get; set; } = "ACT";
    }
}
