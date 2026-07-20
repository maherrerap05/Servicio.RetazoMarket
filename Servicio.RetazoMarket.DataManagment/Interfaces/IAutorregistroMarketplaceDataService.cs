using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Interfaces
{
    public interface IAutorregistroMarketplaceDataService
    {
        Task<AutorregistroMarketplaceResultDataModel> RegistrarAsync(
            AutorregistroMarketplaceDataModel model,
            CancellationToken cancellationToken = default);
    }
}
