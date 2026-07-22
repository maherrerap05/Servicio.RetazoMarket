using System.ComponentModel.DataAnnotations;

namespace Servicio.RetazoMarket.Api.Models.Settings;

public class JwtSettings
{
    [Required]
    [MinLength(32)]
    public string SecretKey { get; set; } = null!;

    [Required]
    public string Issuer { get; set; } = null!;

    [Required]
    public string Audience { get; set; } = null!;

    [Range(1, 1440)]
    public int ExpirationMinutes { get; set; }
}
