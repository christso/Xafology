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
    public class SaveLayoutPopupListViewController : LayoutPopupListViewController
    {

        private SimpleAction saveAction;

        public SaveLayoutPopupListViewController()
        {
            TargetViewId = Data.PivotGridSavedLayoutUISaveListViewId;

            this.saveAction = new SimpleAction(this, "LayoutHeaderSaveAction", "PopupActions");
            this.saveAction.Caption = "Save";
            this.saveAction.Execute += saveAction_Execute;
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
            if (PivotGridLayoutController != null)
                obj.TypeName = PivotGridLayoutController.View.ObjectTypeInfo.Name;
        }
        
        // save pivot grid layout
        private void saveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (PivotGridLayoutController == null)
                throw new ArgumentNullException("pivotGridLayoutController");
            PivotGridLayoutController.SaveLayout((PivotGridSavedLayout)View.CurrentObject);
        }
    }
}