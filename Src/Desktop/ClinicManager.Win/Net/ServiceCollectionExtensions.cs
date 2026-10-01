using ClinicManager.Win.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ClinicManager.Win.Net;

/// <summary>
/// Extension methods for IServiceCollection to add infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services including logging, HTTP clients with resilience policies,
    /// and configuration management to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register configuration
        var appSettings = new ApplicationSettings();
        configuration.Bind(appSettings);
        services.AddSingleton(appSettings);

        // Add Serilog logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: false);
        });

        // Add HTTP clients with resilience policies
        AddPrimaryApiClient(services, appSettings);
        AddSecondaryApiClient(services, appSettings);

        return services;
    }

    /// <summary>
    /// Adds the primary API HTTP client with resilience policies.
    /// </summary>
    private static void AddPrimaryApiClient(IServiceCollection services, ApplicationSettings settings)
    {
        var primaryConfig = settings.HttpClients.PrimaryApi;
        
        services.AddHttpClient("PrimaryApi", client =>
        {
            client.BaseAddress = new Uri(primaryConfig.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(primaryConfig.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "ClinicManager/1.0");
        });
    }

    /// <summary>
    /// Adds the secondary API HTTP client with resilience policies.
    /// </summary>
    private static void AddSecondaryApiClient(IServiceCollection services, ApplicationSettings settings)
    {
        var secondaryConfig = settings.HttpClients.SecondaryApi;
        
        services.AddHttpClient("SecondaryApi", client =>
        {
            client.BaseAddress = new Uri(secondaryConfig.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(secondaryConfig.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "ClinicManager/1.0");
        });
    }

}
