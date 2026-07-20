namespace Servicio.RetazoMarket.Business.DTOs.Auth
{
    public class AutorregistroMarketplaceRequest
    {
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string correo { get; set; } = null!;
        public string telefono { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public string contrasena { get; set; } = null!;
    }
}
