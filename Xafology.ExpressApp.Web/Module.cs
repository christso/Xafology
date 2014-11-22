using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;

namespace Xafology.ExpressApp.Web.SystemModule
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed class XafologySystemAspNetModule : XafologyModuleBase
    {
        public XafologySystemAspNetModule()
        {
            RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.SystemModule.XafologySystemModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule));
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

        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<Xafology.ExpressApp.Editors.IModelCustomUserControlViewItem,
                Xafology.ExpressApp.Web.Editors.IModelWebCustomUserControlViewItem>();
        }
    }
}
