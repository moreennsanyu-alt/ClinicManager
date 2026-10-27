using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security;

namespace PermissionPolicyExample.Win {
    public partial class PermissionPolicyExampleWindowsFormsApplication : WinApplication {
        public PermissionPolicyExampleWindowsFormsApplication() {
            InitializeComponent();
            DelayedViewItemsInitialization = true;
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new SecuredObjectSpaceProvider(
                (SecurityStrategy)Security, args.ConnectionString, args.Connection);
        }
        private void PermissionPolicyExampleWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            throw new InvalidOperationException(
                "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.");
        }
        private void PermissionPolicyExampleWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if (userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
    }
}
