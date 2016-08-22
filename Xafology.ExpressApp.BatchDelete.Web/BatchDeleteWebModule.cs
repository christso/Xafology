using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;

namespace Xafology.ExpressApp.BatchDelete.Web
{
    // You can override various virtual methods and handle corresponding events to manage various aspects of your XAF application UI and behavior.
    public sealed partial class BatchDeleteWebModule : ModuleBase
    { // http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic
        public BatchDeleteWebModule()
        {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }

        private void InitializeComponent()
        {
            // add required modules
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.SystemModule.XafologySystemModule));
        }
    }
}
