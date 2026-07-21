namespace Servicio.RetazoMarket.Business.DTOs.Auth
{
    public class ActorContext
    {
        public int id_usuario { get; set; }
        public int? id_cliente { get; set; }
        public string correo { get; set; } = null!;
        public IReadOnlyCollection<string> roles { get; set; } = Array.Empty<string>();
        public bool autenticado { get; set; }
    }
}
