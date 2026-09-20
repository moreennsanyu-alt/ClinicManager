using ClinicManager.Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#if (UseAspire)
builder.AddServiceDefaults();
#endif

builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

#if (!UseAspire)
app.UseHealthChecks("/health");
#endif
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapOpenApi();
app.MapScalarApiReference();

#if (!UseApiOnly)
app.MapFallbackToFile("index.html");
#endif

app.UseExceptionHandler(options => { });

#if (UseApiOnly)
app.Map("/", () => Results.Redirect("/scalar"));
#endif

#if (UseAspire)
app.MapDefaultEndpoints();
#endif
app.MapEndpoints();

app.Run();

public partial class Program { }
