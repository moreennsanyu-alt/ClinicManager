using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrismDryIocHttpDemo.Http;

namespace PrismDryIocHttpDemo.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoLogging(this IServiceCollection services) =>
        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddDebug(); // shows in the VS "Output" window
        });

    public static IServiceCollection AddDemoHttpClients(this IServiceCollection services)
    {
        // Handlers used with AddHttpMessageHandler<T> must be registered (transient is the norm).
        // LoggingHttpMessageHandler depends on IRequestLogService, which is registered in App.RegisterTypes.
        services.AddTransient<LoggingHttpMessageHandler>();

        services
            .AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            })
            .AddHttpMessageHandler<LoggingHttpMessageHandler>();

        return services;
    }
}
