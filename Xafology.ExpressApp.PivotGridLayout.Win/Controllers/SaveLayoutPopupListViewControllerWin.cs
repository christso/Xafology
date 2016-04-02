using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout.Win.Controllers
{
    public class SaveLayoutPopupListViewControllerWin : ViewController<ListView>
    {
        public SaveLayoutPopupListViewControllerWin()
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
            obj.UIPlatform = UIPlatform.Win;
        }
    }
}
