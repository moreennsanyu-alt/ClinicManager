using Prism.Ioc;

namespace ClinicManager.Win.Features.Infrastructure;

    public class InfrastructureModule : IModule
    {
        public void RegisterTypes(IContainerRegistry registry)
        {
           
           // registry.RegisterSingleton<IOrderService, OrderService>();
           // registry.RegisterForNavigation<OrderListView, OrderListViewModel>();
        }

        public void OnInitialized(IContainerProvider container)
        {
            //var regions = container.Resolve<IRegionManager>();
            //regions.RegisterViewWithRegion(RegionNames.Content, typeof(OrderListView));
        }
    }
