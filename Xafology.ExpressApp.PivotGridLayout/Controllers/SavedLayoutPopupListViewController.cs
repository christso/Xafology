using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public class SavedLayoutPopupListViewController : ViewController<ListView>
    {
        public ViewController SendingViewController;

        public SavedLayoutPopupListViewController()
        {
            TargetObjectType = typeof(PivotGridSavedLayout);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated += newObjectViewController_ObjectCreated;
        }

        void newObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var obj = (PivotGridSavedLayout)e.CreatedObject;
            obj.TypeName = SendingViewController.View.ObjectTypeInfo.Name;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated -= newObjectViewController_ObjectCreated;
        }
    }
}
