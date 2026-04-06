using Prism.Ioc;
using Prism.DryIoc;
using ClinicManager.Win.Views;
using System.Windows;
using SingleInstanceCore;

namespace ClinicManager.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication, ISingleInstance
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
	    
	    protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SingleInstance.Cleanup();
	
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("soheilkd_ExampleIPC");
		    if (!isFirstInstance)
		    {
			    //If it's not the first instance, arguments are automatically passed to the first instance
			    //OnInstanceInvoked will be raised on the first instance
		    	//You may shut down the current instance
			    Current.Shutdown();
		     }
            base.OnStartup(e);

        }
        
        public void OnInstanceInvoked(string[] args)
	    {
		    // What to do with the args another instance has sent
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
    }
}
