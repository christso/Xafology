using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout.Web.Controllers
{
    /// <summary>
    /// Enable NewAction in Popup Window in ASP.NET since it is disabled by default.
    /// </summary>
    public class PivotGridSavedLayoutWebNewObjectViewController : WebNewObjectViewController
    {
        protected override void UpdateActionActivity(ActionBase action)
        {
            base.UpdateActionActivity(action);
            action.Active.RemoveItem("PopupWindowContext");
            action.Active.Clear();
        }
    }
}
