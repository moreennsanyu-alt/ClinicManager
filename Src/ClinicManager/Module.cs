using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using System.Reflection;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace ClinicManager {
    public sealed partial class ClinicManagerModule : ModuleBase {
        public ClinicManagerModule() {
            InitializeComponent();
        }
        protected override IEnumerable<Type> GetDeclaredExportedTypes() {
            List<Type> result = new List<Type>(base.GetDeclaredExportedTypes());
            result.AddRange(new Type[] { typeof(PermissionPolicyUser), typeof(PermissionPolicyRole) });
            return result;
        }
    }
}
