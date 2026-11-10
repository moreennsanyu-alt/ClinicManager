using System.ServiceProcess;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp;
using System.Collections;
using DevExpress.ExpressApp.Xpo;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.MiddleTier;

namespace ClinicManager.Server
{
    public class CMService : ServiceBase
    {
        public const string Name = "CMServer";
        WcfXafServiceHost serviceHost = null;
        static CMService()
        {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
        
        public CMService()
        {
            ServiceName = Name;
        }

        // Public wrappers so console mode can call the protected overrides.
        public void StartConsole(string[] args) => OnStart(args);
        public void StopConsole() => OnStop();

        protected override void OnStart(string[] args)
        {
                 try
            {
                SecurityAdapterHelper.Enable();
                ValueManager.ValueManagerType = typeof(MultiThreadValueManager<>).GetGenericTypeDefinition();

                //string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string connectionString = InMemoryDataStoreProvider.ConnectionString;

                ServerApplication serverApplication = new ServerApplication();
                serverApplication.ApplicationName = ClinicManagerModule.AppName;
                serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                    
                serverApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;

                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new SystemWindowsFormsModule());
                serverApplication.Modules.Add(new ClinicManager());
                serverApplication.Modules.Add(new SecurityModule());
                serverApplication.Modules.EndInit();

                serverApplication.DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(serverApplication_DatabaseVersionMismatch);
                serverApplication.CreateCustomObjectSpaceProvider += new EventHandler<CreateCustomObjectSpaceProviderEventArgs>(serverApplication_CreateCustomObjectSpaceProvider);

                serverApplication.ConnectionString = connectionString;
                Console.WriteLine("Setup...");
                serverApplication.Setup();
                Console.WriteLine("CheckCompatibility...");
                serverApplication.CheckCompatibility();
                serverApplication.Dispose();

                Console.WriteLine("Starting server...");
                Func<IDataServerSecurity> dataServerSecurityProvider = () =>
                {
                    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), new AuthenticationStandard());
                    security.SupportNavigationPermissionsForTypes = false;
                    return security;
                };

                 serviceHost = new WcfXafServiceHost(connectionString, dataServerSecurityProvider);
                serviceHost.AddServiceEndpoint(typeof(IWcfXafDataServer), WcfDataServerHelper.CreateNetTcpBinding(), "net.tcp://127.0.0.1:1451/DataServer");
                serviceHost.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurs: " + e.Message);
                Console.WriteLine("Press Enter to close.");
                Console.ReadLine();
            }
        }

        protected override void OnStop()
        {
            serviceHost.Close();
            serviceHost=null;
                
        }
        
        private static void serverApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
            e.Updater.Update();
            e.Handled = true;
        }
        private static void serverApplication_CreateCustomObjectSpaceProvider(object sender, CreateCustomObjectSpaceProviderEventArgs e)
        {
            e.ObjectSpaceProvider = new XPObjectSpaceProvider(e.ConnectionString, e.Connection);
        }
    }
}
