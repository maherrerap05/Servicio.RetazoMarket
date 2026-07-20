namespace Servicio.RetazoMarket.Business.DTOs.MetodoPago
{
    public class MetodoPagoResponse
    {
        public int id_metodo { get; set; }
        public string met_nombre { get; set; } = null!;
        public string codigo_sri { get; set; } = null!;
    }
}
