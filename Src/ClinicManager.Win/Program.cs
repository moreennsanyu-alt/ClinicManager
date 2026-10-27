using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using System.ServiceModel;

namespace PermissionPolicyExample.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if EASYTEST
			DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if (Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder)
            {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            PermissionPolicyExampleWindowsFormsApplication winApplication = new PermissionPolicyExampleWindowsFormsApplication();
            // Refer to the https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112680.aspx help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            try
            {
                string connectionString = "net.tcp://127.0.0.1:1451/DataServer";
                WcfSecuredClient wcfSecuredClient = new WcfSecuredClient(WcfDataServerHelper.CreateNetTcpBinding(), new EndpointAddress(connectionString));
                MiddleTierClientSecurity security = new MiddleTierClientSecurity(wcfSecuredClient);
                security.IsSupportChangePassword = true;
                winApplication.Security = security;
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.Never;
                winApplication.CreateCustomObjectSpaceProvider +=
                    delegate (object sender, CreateCustomObjectSpaceProviderEventArgs e)
                    {
                        e.ObjectSpaceProvider = new MiddleTierServerObjectSpaceProvider(wcfSecuredClient);
                    };
                winApplication.Setup();
                winApplication.Start();
                wcfSecuredClient.Dispose();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}
