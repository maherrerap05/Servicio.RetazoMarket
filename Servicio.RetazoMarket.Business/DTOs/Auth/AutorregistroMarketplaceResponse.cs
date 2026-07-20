using Servicio.RetazoMarket.Business.DTOs.Cliente;
using Servicio.RetazoMarket.Business.DTOs.Usuario;

namespace Servicio.RetazoMarket.Business.DTOs.Auth
{
    public class AutorregistroMarketplaceResponse
    {
        public ClienteResponse Cliente { get; set; } = null!;
        public UsuarioResponse Usuario { get; set; } = null!;
    }
}
