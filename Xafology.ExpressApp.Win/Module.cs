using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;

namespace Xafology.ExpressApp.Win.SystemModule
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class XafologySystemWindowsFormsModule : XafologyModuleBase
    {
        public XafologySystemWindowsFormsModule()
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

        // Extends the Application Model elements for View and Navigation Items to be able to specify custom controls via the Model Editor.
        // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument3169 help article for more information.
        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<Xafology.ExpressApp.Editors.IModelCustomUserControlViewItem,
                Xafology.ExpressApp.Win.Editors.IModelWinCustomUserControlViewItem>();
        }
    }
}
