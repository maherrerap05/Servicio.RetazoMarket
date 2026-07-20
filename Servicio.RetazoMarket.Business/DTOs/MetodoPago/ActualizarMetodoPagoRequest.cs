namespace Servicio.RetazoMarket.Business.DTOs.MetodoPago
{
    public class ActualizarMetodoPagoRequest
    {
        public int id_metodo { get; set; }
        public string met_nombre { get; set; } = null!;
        public string codigo_sri { get; set; } = null!;
    }
}
