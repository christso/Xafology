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

namespace Xafology.PivotGridLayoutDemo.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PopupViewController : ViewController
    {
        public PopupViewController()
        {
            var myAction = new SingleChoiceAction(this, "MyActions", PredefinedCategory.ObjectsCreation);
            myAction.Caption = "MyAction";
            myAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            myAction.Execute += myAction_Execute;

            var dataAction = new ChoiceActionItem();
            dataAction.Caption = "Init Data";
            myAction.Items.Add(dataAction);

            var showDashboardAction = new ChoiceActionItem();
            showDashboardAction.Caption = "Show Dashboard";
            myAction.Items.Add(showDashboardAction);
        }

        private void myAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case "Show Dashboard":

                    IObjectSpace objectSpace = Application.CreateObjectSpace();

                    var dview = Application.CreateDashboardView(objectSpace, "PivotGridLayoutDemoDashboardView", true);
                    ShowViewParameters svp = e.ShowViewParameters;
                    svp.TargetWindow = TargetWindow.NewModalWindow;
                    svp.CreatedView = dview;
                    break;

                case "Init Data":
                    var data = new DataInitializer(ObjectSpace);
                    data.Run();
                    ObjectSpace.CommitChanges();
                    break;
            }
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
