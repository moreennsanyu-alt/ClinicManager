using Prism.Ioc;

namespace ClinicManager.Win.Features.Infrastructure;

public static class InfrastructureFeatureExtensions
{
    /// <summary>Registers everything the Orders feature needs.</summary>
    public static IContainerRegistry AddInfrastructureFeature(this IContainerRegistry containerRegistry)
    {
        return containerRegistry
            .AddInfrastructureServices()
            .AddInfrastructureViews();
    }

    public static IContainerRegistry AddInfrastructureServices(this IContainerRegistry containerRegistry)
    {
        //containerRegistry.RegisterSingleton<IOrderRepository, OrderRepository>();
       // containerRegistry.Register<IOrderService, OrderService>(); // transient
        return containerRegistry;
    }

    public static IContainerRegistry AddInfrastructureViews(this IContainerRegistry containerRegistry)
    {
        // Navigation views (view + view model, key = view type name unless overridden)
       // containerRegistry.RegisterForNavigation<OrderListView, OrderListViewModel>();
        ///containerRegistry.RegisterForNavigation<OrderDetailView, OrderDetailViewModel>("OrderDetail");

        // Dialogs
        //containerRegistry.RegisterDialog<ConfirmOrderDialog, ConfirmOrderDialogViewModel>();

        return containerRegistry;
    }
}
