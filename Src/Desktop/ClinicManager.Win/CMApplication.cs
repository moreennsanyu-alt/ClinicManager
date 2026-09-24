using System.Windows;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;

namespace ClinicManager.Win;

public abstract class CMApplication : PrismApplication
{
    /// <summary>
    /// Build the DryIoc container ourselves so we can pour an <see cref="IServiceCollection"/> into it
    /// *before* Prism starts registering its own types. The returned container is then handed to Prism.
    /// After this, there is ONE container: anything registered through Microsoft DI (IHttpClientFactory,
    /// ILogger&lt;T&gt;, typed clients...) can be injected into Prism view models, and anything registered
    /// through Prism (see <see cref="RegisterTypes"/>) can be injected into Microsoft DI-created objects
    /// such as the HTTP logging handler.
    /// </summary>
    protected override IContainerExtension CreateContainerExtension()
    {
        var services = new ServiceCollection();
        services.AddDefaultLogging();
        services.AddHttpClients();

        IContainer container = new DryIoc.Container(CreateContainerRules())
            .WithDependencyInjectionAdapter(services);

        return new DryIocContainerExtension(container);
    }

    protected override Rules CreateContainerRules()
    {
        return Rules.Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                        .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                        .WithFuncAndLazyWithoutRegistration()
                        .WithTrackingDisposableTransients()
                        //.WithoutFastExpressionCompiler()
                        .WithFactorySelector(Rules.SelectLastRegisteredFactory());
    }

    // App.xaml.cs
    protected override void ConfigureModuleCatalog(IModuleCatalog catalog)
    {
        var moduleTypes = typeof(CMApplication).Assembly
            .GetTypes()
            .Where(t => typeof(IModule).IsAssignableFrom(t)
                    && t is { IsAbstract: false, IsInterface: false });

        foreach (var type in moduleTypes)
            catalog.AddModule(type);   // WhenAvailable by default
    }

}
