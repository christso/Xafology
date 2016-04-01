using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.PivotGridLayoutDemo.Module.Web.Controllers
{
    public class PivotGridLayoutDashboardController : ViewController<DashboardView>
    {

        private SimpleAction saveAction;
        private SimpleAction loadAction;

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

        protected override void OnActivated()
        {
            base.OnActivated();

            var listViewItem = (DashboardViewItem)View.FindItem(Data.PivotGridLayoutListViewViewItemId);
            listViewItem.ControlCreated += listViewItem_ControlCreated;
        }

        private void listViewItem_ControlCreated(object sender, EventArgs e)
        {
            
        }

        private void loadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void saveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
        }
    }
}
