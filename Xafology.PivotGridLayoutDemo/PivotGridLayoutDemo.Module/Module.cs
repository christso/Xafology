using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;

using Xafology.ExpressApp.PivotGridLayout;
using PivotGridLayoutDemo.Module.BusinessUtils;
using PivotGridLayoutDemo.Module.BusinessObjects;

namespace PivotGridLayoutDemo.Module
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class PivotGridLayoutDemoModule : ModuleBase
    {
        public PivotGridLayoutDemoModule()
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
            
            // names reserved by the system
            PivotGridSavedLayout.ReservedLayoutNames.Add(DomainObject1PivotGridSetup.Default1LayoutName, typeof(DomainObject1).Name);
            PivotGridSavedLayout.ReservedLayoutNames.Add(DomainObject1PivotGridSetup.Default2LayoutName, typeof(DomainObject1).Name);
        }
   
    }
}
