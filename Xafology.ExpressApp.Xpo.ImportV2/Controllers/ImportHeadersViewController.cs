using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ImportHeadersViewController : ViewController
    {
        public ImportHeadersViewController()
        {
            TargetObjectType = typeof(ImportHeadersParam);
            var importAction = new SingleChoiceAction(this, "ImportAction", PredefinedCategory.ObjectsCreation);
            importAction.Caption = "Import";
            importAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            importAction.Execute += ImportAction_Execute;

            var runAction = new ChoiceActionItem();
            runAction.Caption = "Run";
            importAction.Items.Add(runAction);

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void ImportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
