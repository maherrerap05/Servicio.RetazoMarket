using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Api.Models.Settings;
using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Api.Controllers.V1.Internal.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAutorregistroMarketplaceService _autorregistroService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        IAuthService authService,
        IAutorregistroMarketplaceService autorregistroService,
        IOptions<JwtSettings> jwtOptions)
    {
        _authService = authService;
        _autorregistroService = autorregistroService;
        _jwtSettings = jwtOptions.Value;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, result.IdUsuario.ToString()),
            new("name", result.UserName),
            new(JwtRegisteredClaimNames.Email, result.Correo.Trim().ToLowerInvariant()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (result.IdCliente is > 0)
            claims.Add(new Claim("id_cliente", result.IdCliente.Value.ToString()));

        claims.AddRange(result.Roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim().ToUpperInvariant())
            .Distinct(StringComparer.Ordinal)
            .Select(role => new Claim("role", role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        result.Token = new JwtSecurityTokenHandler().WriteToken(token);
        result.ExpirationUtc = expiration;

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso."));
    }

    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(typeof(ApiResponse<AutorregistroMarketplaceResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(
        [FromBody] AutorregistroMarketplaceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _autorregistroService.RegistrarAsync(request, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AutorregistroMarketplaceResponse>.Ok(
                result,
                "Registro completado exitosamente."));
    }
}
