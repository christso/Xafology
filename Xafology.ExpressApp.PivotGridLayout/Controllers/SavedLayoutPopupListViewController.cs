using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public class SavedLayoutPopupListViewController : ViewController<ListView>
    {
        public SavedLayoutPopupListViewController()
        {
            TargetObjectType = typeof(PivotGridSavedLayout);
        }

        protected override void OnActivated()
        {
            base.OnActivated();


        }
    }
}
