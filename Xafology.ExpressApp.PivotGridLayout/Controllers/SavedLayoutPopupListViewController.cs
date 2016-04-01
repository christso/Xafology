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
    public class SavedLayoutPopupListViewController : ViewController<ListView>
    {
        public const string PivotGridLayoutListViewViewItemId = "PivotGridLayoutListViewViewItem";

        private SimpleAction saveAction;
        private SimpleAction loadAction;

        public SavedLayoutPopupListViewController()
        {
            TargetViewId = Data.PivotGridSavedLayoutUISaveListViewId;

            this.saveAction = new SimpleAction(this, "LayoutHeaderSaveAction", "PopupActions");
            this.saveAction.Caption = "Save";
            this.saveAction.Execute += saveAction_Execute;

            this.loadAction = new SimpleAction(this, "LayoutHeaderLoadAction", "PopupActions");
            this.loadAction.Caption = "Load";
            this.loadAction.Execute += loadAction_Execute;
        }

        private PivotGridLayoutController pivotGridLayoutController;

        // property injection
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

            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated += newObjectViewController_ObjectCreated;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated -= newObjectViewController_ObjectCreated;
        }

        // enter default object type name for new object
        private void newObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var obj = (PivotGridSavedLayout)e.CreatedObject;
            if (pivotGridLayoutController != null)
                obj.TypeName = pivotGridLayoutController.View.ObjectTypeInfo.Name;
        }

        // load pivot grid layout
        private void loadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (pivotGridLayoutController == null)
                throw new ArgumentNullException("pivotGridLayoutController");
            pivotGridLayoutController.LoadLayout((PivotGridSavedLayout)View.CurrentObject);
        }

        // save pivot grid layout
        private void saveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (pivotGridLayoutController == null)
                throw new ArgumentNullException("pivotGridLayoutController");
            pivotGridLayoutController.SaveLayout((PivotGridSavedLayout)View.CurrentObject);
        }

    }
}