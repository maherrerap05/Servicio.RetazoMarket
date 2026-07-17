namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MetodoPagoDataModel
    {
        public int id_metodo { get; set; }
        public string met_nombre { get; set; } = null!;
        public string codigo_sri { get; set; } = null!;
    }
}
