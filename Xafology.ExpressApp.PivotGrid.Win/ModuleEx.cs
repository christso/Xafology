using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.PivotGrid.Win
{
    public sealed partial class XafologyPivotGridWinModule
    {
        // Registers custom extensions for Application Model elements.
        // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument3169 help article for more information.
        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<Xafology.ExpressApp.PivotGrid.Editors.IModelPivotGridViewItem,
                Xafology.ExpressApp.PivotGrid.Win.Editors.IModelWinPivotGridViewItem>();
        }
    }
}
