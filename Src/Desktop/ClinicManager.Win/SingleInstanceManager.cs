// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualBasic.ApplicationServices;

namespace ClinicManager.Win
{
    public class SingleInstanceManager : WindowsFormsApplicationBase
    {
        private SingleInstanceApplication _app;

        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs e)
        {
            // First time app is launched
            _app = new ClinicManagerApp();
            _app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches
            base.OnStartupNextInstance(eventArgs);
            _app.Activate();
        }
    }
}