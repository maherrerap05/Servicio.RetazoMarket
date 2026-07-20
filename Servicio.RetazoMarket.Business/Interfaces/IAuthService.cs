using Servicio.RetazoMarket.Business.DTOs.Auth;
namespace Servicio.RetazoMarket.Business.Interfaces;
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
