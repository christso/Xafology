using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;

namespace Xafology.ExcelReportDemo.Module
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class ExcelReportDemoModule : ModuleBase
    {
        public ExcelReportDemoModule()
        {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.

        }
    }
}
