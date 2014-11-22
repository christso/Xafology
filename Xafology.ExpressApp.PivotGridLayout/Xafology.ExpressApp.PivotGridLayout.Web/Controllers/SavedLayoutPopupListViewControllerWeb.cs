using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout.Web.Controllers
{
    public class SavedLayoutPopupListViewControllerWeb : ViewController<ListView>
    {
        public SavedLayoutPopupListViewControllerWeb()
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
            obj.UIPlatform = UIPlatform.Web;
        }
    }
}
