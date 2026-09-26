using Prism.Ioc;

namespace ClinicManager.Win.Features.Infrastructure;

    public class InfrastructureModule : IModule
    {
        public void RegisterTypes(IContainerRegistry registry)
        {
           var nav = containerProvider.Resolve<INavigationTreeBuilder>();

        nav.Register(new NavigationItem
        {
            Key = "AllUsers",
            Title = "All Users",
            Path = "Users/AllUsers",
            ViewName = nameof(HomeView)
        });

        nav.Register(new NavigationItem
        {
            Key = "Detail",
            Title = "Detail",
            Path = "Users/Detail",
            ViewName = nameof(HomeView)
        });
           // registry.RegisterSingleton<IOrderService, OrderService>();
           // registry.RegisterForNavigation<OrderListView, OrderListViewModel>();
        }

        public void OnInitialized(IContainerProvider container)
        {
            //var regions = container.Resolve<IRegionManager>();
            //regions.RegisterViewWithRegion(RegionNames.Content, typeof(OrderListView));
        }
    }
