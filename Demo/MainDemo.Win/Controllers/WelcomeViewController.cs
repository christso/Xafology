using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainDemo.Win.Controllers
{
    public class WelcomeViewController : ViewController<DashboardView>
    {
        public WelcomeViewController()
        {
            TargetViewId = "Welcome_Dashboard";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            foreach (var item in View.Items)
            {
                
            }
        }

    }
}
