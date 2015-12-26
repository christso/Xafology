using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;

namespace Xafology.ExpressApp.Xpo
{
    public class XpoModule : ModuleBase
    {
        public XpoModule()
        {

        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
        }

        private void InitializeComponent()
        {
            // 
            // XpoModule
            // 
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.SystemModule.XafologySystemModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));

        }
    }
}
