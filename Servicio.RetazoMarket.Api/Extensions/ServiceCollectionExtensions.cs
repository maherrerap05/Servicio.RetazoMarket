using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.Business.Interfaces;
using Servicio.RetazoMarket.Business.Services;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Services;

namespace Servicio.RetazoMarket.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RetazoMarketDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró la cadena de conexión ConnectionStrings:RetazoMarketDb.");
        }

        services.AddDbContext<RetazoMarketDbContext>(options =>
            options.UseNpgsql(connectionString));

        // =========================
        // UNIT OF WORK
        // =========================
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // =========================
        // DATA MANAGMENT
        // =========================
        services.AddScoped<IAutorregistroMarketplaceDataService, AutorregistroMarketplaceDataService>();
        services.AddScoped<ICategoriaMaterialDataService, CategoriaMaterialDataService>();
        services.AddScoped<IClienteDataService, ClienteDataService>();
        services.AddScoped<IDescuentoAdministracionDataService, DescuentoAdministracionDataService>();
        services.AddScoped<IDescuentoDataService, DescuentoDataService>();
        services.AddScoped<IFavoritoDataService, FavoritoDataService>();
        services.AddScoped<IImagenAdministracionDataService, ImagenAdministracionDataService>();
        services.AddScoped<IImagenDataService, ImagenDataService>();
        services.AddScoped<ILineaDataService, LineaDataService>();
        services.AddScoped<IMaterialDataService, MaterialDataService>();
        services.AddScoped<IMetodoPagoDataService, MetodoPagoDataService>();
        services.AddScoped<IMovimientoMaterialDataService, MovimientoMaterialDataService>();
        services.AddScoped<IMovimientoProductoDataService, MovimientoProductoDataService>();
        services.AddScoped<IPedidoAdministracionDataService, PedidoAdministracionDataService>();
        services.AddScoped<IPedidoDataService, PedidoDataService>();
        services.AddScoped<IPersonalizacionDataService, PersonalizacionDataService>();
        services.AddScoped<IProductoDataService, ProductoDataService>();
        services.AddScoped<IProductoFabricacionDataService, ProductoFabricacionDataService>();
        services.AddScoped<IProductoMaterialDataService, ProductoMaterialDataService>();
        services.AddScoped<IProductoPedidoDataService, ProductoPedidoDataService>();
        services.AddScoped<IProveedorDataService, ProveedorDataService>();
        services.AddScoped<IProveedorMaterialDataService, ProveedorMaterialDataService>();
        services.AddScoped<IRolDataService, RolDataService>();
        services.AddScoped<IUsuarioDataService, UsuarioDataService>();

        // =========================
        // BUSINESS
        // =========================
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAutorizacionService, AutorizacionService>();
        services.AddScoped<IAutorregistroMarketplaceService, AutorregistroMarketplaceService>();
        services.AddScoped<ICategoriaMaterialService, CategoriaMaterialService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IDescuentoService, DescuentoService>();
        services.AddScoped<IFavoritoService, FavoritoService>();
        services.AddScoped<IImagenService, ImagenService>();
        services.AddScoped<ILineaService, LineaService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IMetodoPagoService, MetodoPagoService>();
        services.AddScoped<IMovimientoMaterialService, MovimientoMaterialService>();
        services.AddScoped<IMovimientoProductoService, MovimientoProductoService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IPersonalizacionService, PersonalizacionService>();
        services.AddScoped<IProductoMarketplaceService, ProductoMarketplaceService>();
        services.AddScoped<IProductoMaterialService, ProductoMaterialService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IProveedorMaterialService, ProveedorMaterialService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IUsuarioService, UsuarioService>();

        return services;
    }
}
