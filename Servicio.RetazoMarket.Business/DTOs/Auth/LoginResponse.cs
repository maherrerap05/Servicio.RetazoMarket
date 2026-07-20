namespace Servicio.RetazoMarket.Business.DTOs.Auth;
public class LoginResponse
{
    public string UserName { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public bool Activo { get; set; }
    public int? IdCliente { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationUtc { get; set; } = DateTime.MinValue;
}
