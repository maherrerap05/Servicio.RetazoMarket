namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ClienteDataModel
    {
        public int id_cliente { get; set; }
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string correo { get; set; } = null!;
        public string telefono { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public string origen { get; set; } = null!;
        public string cli_estado { get; set; } = null!;
        public DateTime fecha_registro { get; set; }
    }
}
