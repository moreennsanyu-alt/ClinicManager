using ServiceStack;

[assembly: HostingStartup(typeof(ClinicManager.ConfigureServerEvents))]

namespace ClinicManager;

public class ConfigureServerEvents : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            services.AddPlugin(new ServerEventsFeature());
        });
}
