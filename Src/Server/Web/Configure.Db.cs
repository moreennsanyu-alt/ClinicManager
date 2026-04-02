using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ClinicManager.Data;
using Microsoft.EntityFrameworkCore;

[assembly: HostingStartup(typeof(ClinicManager.ConfigureDb))]

namespace ClinicManager;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                ?? "DataSource=App_Data/app.db;Cache=Shared";
            
            services.AddOrmLite(options => options.UseSqlite(connectionString));

            // $ dotnet ef migrations add CreateIdentitySchema
            // $ dotnet ef database update
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString, b => b.MigrationsAssembly(nameof(ClinicManager))));
            
            // Enable built-in Database Admin UI at /admin-ui/database
            services.AddPlugin(new AdminDatabaseFeature());
        });
}
