namespace Servicio.RetazoMarket.Business.DTOs.Linea
{
    public class ActualizarLineaRequest
    {
        public int id_linea { get; set; }
        public string lin_nombre { get; set; } = null!;
        public string lin_estado { get; set; } = null!;
    }
}
