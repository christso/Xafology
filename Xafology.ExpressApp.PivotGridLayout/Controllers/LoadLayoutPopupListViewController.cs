using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public class LoadLayoutPopupListViewController : LayoutPopupListViewController
    {
        private SimpleAction loadAction;

        public LoadLayoutPopupListViewController()
        {
            TargetViewId = Data.PivotGridSavedLayoutUILoadListViewId;

            this.loadAction = new SimpleAction(this, "LayoutHeaderLoadAction", PredefinedCategory.ObjectsCreation);
            this.loadAction.Caption = "Load";
            this.loadAction.Execute += loadAction_Execute;
        }

        // load pivot grid layout
        private void loadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (PivotGridLayoutController == null)
                throw new ArgumentNullException("pivotGridLayoutController");
            PivotGridLayoutController.LoadLayout((PivotGridSavedLayout)View.CurrentObject);
        }
        
    }
}