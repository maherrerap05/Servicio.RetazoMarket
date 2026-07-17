namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ClienteFiltroDataModel
    {
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? origen { get; set; }
        public string? estado { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
