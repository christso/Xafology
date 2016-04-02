using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public abstract class LayoutPopupListViewController : ViewController<ListView>
    {
        // property injection
        public PivotGridLayoutController PivotGridLayoutController { get; set; }
    }
}
