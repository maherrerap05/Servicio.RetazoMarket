namespace Servicio.RetazoMarket.Api.Extensions;

public static class AuthorizationExtensions
{
    public const string SuperAdministradorPolicy = "SuperAdministrador";
    public const string AdministradorPolicy = "Administrador";
    public const string ClientePolicy = "Cliente";

    private const string SuperAdministradorRole = "SUPERADMINISTRADOR";
    private const string AdministradorRole = "ADMINISTRADOR";
    private const string ClienteRole = "CLIENTE";

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(SuperAdministradorPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(SuperAdministradorRole);
            })
            .AddPolicy(AdministradorPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(SuperAdministradorRole, AdministradorRole);
            })
            .AddPolicy(ClientePolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(ClienteRole);
            });

        return services;
    }
}
