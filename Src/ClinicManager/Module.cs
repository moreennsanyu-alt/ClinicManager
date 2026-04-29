using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Chart.Win;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.PivotChart.Win;
using DevExpress.ExpressApp.PivotGrid.Win;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.ExpressApp.ScriptRecorder.Win;
using DevExpress.ExpressApp.TreeListEditors;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.ExpressApp.Win.SystemModule;
using Updater = ClinicManager.DatabaseUpdate.Updater;

namespace ClinicManager;

    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed class ClinicManagerWinModule : ModuleBase {
        public ClinicManagerWinModule() {

            RequiredModuleTypes.Add(typeof(SystemWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(PivotChartWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(PivotGridWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(ChartWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(SchedulerWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(ReportsModuleV2));
            RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
            RequiredModuleTypes.Add(typeof(ViewVariantsModule));
            RequiredModuleTypes.Add(typeof(ReportsV2WinModule));
            RequiredModuleTypes.Add(typeof(DashboardWindowsFormsModule));
            RequiredModuleTypes.Add(typeof(TreeListEditorsModuleBase));
            RequiredModuleTypes.Add(typeof(TreeListEditorsWindowsFormsModule));
            
        }

        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new Updater(objectSpace, versionFromDB);
            return new[] {updater};
        }
    }
