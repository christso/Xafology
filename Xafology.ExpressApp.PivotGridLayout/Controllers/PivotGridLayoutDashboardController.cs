using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public class PivotGridLayoutDashboardController : ViewController<DashboardView>
    {
        public const string PivotGridLayoutListViewViewItemId = "PivotGridLayoutListViewViewItem";

        private SimpleAction saveAction;
        private SimpleAction loadAction;
        private PivotGridLayoutController pivotGridLayoutController;

        public PivotGridLayoutDashboardController()
        {
            TargetViewId = Data.PivotGridLayoutDashboardViewId;

            this.saveAction = new SimpleAction(this, "LayoutHeaderSaveAction", "PopupActions");
            this.saveAction.Caption = "Save";
            this.saveAction.Execute += saveAction_Execute;

            this.loadAction = new SimpleAction(this, "LayoutHeaderLoadAction", "PopupActions");
            this.loadAction.Caption = "Load";
            this.loadAction.Execute += loadAction_Execute;
        }

        public PivotGridLayoutController PivotGridLayoutController
        {
            get
            {
                return this.pivotGridLayoutController;
            }
            set
            {
                this.pivotGridLayoutController = value;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var listViewItem = (DashboardViewItem)View.FindItem(PivotGridLayoutListViewViewItemId);
            listViewItem.ControlCreated += listViewItem_ControlCreated;
        }

        private void newObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var obj = (PivotGridSavedLayout)e.CreatedObject;
            if (PivotGridLayoutController != null)
                obj.TypeName = PivotGridLayoutController.View.ObjectTypeInfo.Name;
        }

        private void listViewItem_ControlCreated(object sender, EventArgs e)
        {
            // get list view "PivotGridSavedLayoutUISave_ListView"
            var listView = (ListView)((DashboardViewItem)sender).InnerView;
            var listViewItem = (DashboardViewItem)View.FindItem(PivotGridLayoutListViewViewItemId);

            // set filter

            var listViewController = listViewItem.Frame.GetController<SavedLayoutPopupListViewController>();

            if (PivotGridLayoutController != null)
                listView.CollectionSource.Criteria["UIFilter"] = PivotGridLayoutController.SavedLayoutUICriteria;

            // new object event handler

            var newObjectViewController = listViewController.Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated += newObjectViewController_ObjectCreated;

        }

        private void loadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var listViewItem = (DashboardViewItem)View.FindItem(PivotGridLayoutListViewViewItemId);
            var listView = listViewItem.InnerView;
            PivotGridLayoutController.LoadLayout((PivotGridSavedLayout)listView.CurrentObject);
        }

        private void saveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var listViewItem = (DashboardViewItem)View.FindItem(PivotGridLayoutListViewViewItemId);
            var listView = listViewItem.InnerView;

            PivotGridLayoutController.SaveLayout((PivotGridSavedLayout)listView.CurrentObject);
        }
    }
}
