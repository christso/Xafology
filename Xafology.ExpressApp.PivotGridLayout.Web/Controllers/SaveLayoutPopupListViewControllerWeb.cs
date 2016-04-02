using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xafology.ExpressApp.PivotGridLayout.Controllers;

namespace Xafology.ExpressApp.PivotGridLayout.Web.Controllers
{
    public class SaveLayoutPopupListViewControllerWeb : ViewController<ListView>
    {
        public SaveLayoutPopupListViewControllerWeb()
        {
            TargetViewId = Data.PivotGridSavedLayoutUISaveListViewId;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated += newObjectViewController_ObjectCreated;
        }

        // default values for new object
        void newObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var obj = (PivotGridSavedLayout)e.CreatedObject;
            obj.UIPlatform = UIPlatform.Web;
        }

        protected override void OnDeactivated()
        {
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated -= newObjectViewController_ObjectCreated;

            base.OnDeactivated();
        }
    }
}
