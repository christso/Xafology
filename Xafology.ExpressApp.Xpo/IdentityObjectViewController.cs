using Xafology.ExpressApp.Xpo;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.IdentitySequenceDemo.Module.Controllers
{
    public class IdentityObjectViewController : ViewController
    {
        public IdentityObjectViewController()
        {
            TargetObjectType = typeof(IdentityBaseObject);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.ObjectSpace.Committed += ObjectSpace_Committed;
        }

        void ObjectSpace_Committed(object sender, EventArgs e)
        {
            var detailView = View as DetailView;
            if (detailView != null)
            {
                ((IdentityBaseObject)detailView.CurrentObject).Reload();
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            View.ObjectSpace.Committed -= ObjectSpace_Committed;
        }
    }
}
