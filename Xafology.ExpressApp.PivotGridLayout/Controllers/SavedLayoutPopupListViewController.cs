 using DevExpress.ExpressApp;

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