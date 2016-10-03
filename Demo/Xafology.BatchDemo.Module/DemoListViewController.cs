using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.BatchDemo.Module
{
    public class DemoListViewController : ViewController
    {
        public DemoListViewController()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var batchDeleteController = Frame.GetController<Xafology.ExpressApp.BatchDelete.BatchDeleteListViewController>();
            batchDeleteController.DeleteFilteredChoiceEnabled = false;
        }
    }
}
