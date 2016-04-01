using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.PivotGridLayout;
using Xafology.ExpressApp.PivotGridLayout.Controllers;

namespace PivotGridLayoutDemo2.Module.Web.Controllers
{
    public class TestViewController : ViewController
    {
        public TestViewController()
        {
            var myAction = new SingleChoiceAction(this, "MyAction", PredefinedCategory.ObjectsCreation);
            myAction.Caption = "MyAction";
            myAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            myAction.Execute += myAction_Execute;
            var showDashboardChoice = new ChoiceActionItem();
            showDashboardChoice.Caption = "Show Dashboard";
            myAction.Items.Add(showDashboardChoice);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        private void myAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            var listViewId = "PivotGridSavedLayoutUISave_ListView";
            var collectionSource = new CollectionSource(objectSpace, typeof(PivotGridSavedLayout));
            var dview = Application.CreateListView(
                listViewId,
                collectionSource,
                true);
            ShowViewParameters svp = e.ShowViewParameters;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.CreatedView = dview;

        }
    }
}
