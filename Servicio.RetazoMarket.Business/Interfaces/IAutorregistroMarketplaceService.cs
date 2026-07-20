using Servicio.RetazoMarket.Business.DTOs.Auth;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface IAutorregistroMarketplaceService
    {
        Task<AutorregistroMarketplaceResponse> RegistrarAsync(
            AutorregistroMarketplaceRequest request,
            CancellationToken cancellationToken = default);
    }
}
