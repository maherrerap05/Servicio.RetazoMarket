namespace Servicio.RetazoMarket.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "CorsPolicy";

    public static IServiceCollection AddCustomCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>()?
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? Array.Empty<string>();

        if (allowedOrigins.Length == 0)
        {
            throw new InvalidOperationException(
                "Debe configurar al menos un origen permitido en Cors:AllowedOrigins.");
        }

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .WithHeaders("Authorization", "Content-Type", "Accept")
                    .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS");
            });
        });

        return services;
    }
}
