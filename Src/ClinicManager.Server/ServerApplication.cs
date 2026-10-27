using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule;
using PermissionPolicyExample.Module;
using DevExpress.ExpressApp.Security;

namespace ConsoleApplicationServer {
    public class ConsoleApplicationServerServerApplication : ServerApplication {
        public ConsoleApplicationServerServerApplication() {
            // Change the ServerApplication.ApplicationName property value. It should be the same as your client application name. 
            this.ApplicationName = "PermissionPolicyExample";

            // Add your client application's modules to the ServerApplication.Modules collection here. 
          
        }
        protected override void OnDatabaseVersionMismatch(DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs args) {
            args.Updater.Update();
            args.Handled = true;
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection);
        }
    }
}
