using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Win.SystemModule
{
    public sealed partial class XafologySystemWindowsFormsModule
    {
        // Extends the Application Model elements for View and Navigation Items to be able to specify custom controls via the Model Editor.
        // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument3169 help article for more information.
        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<Xafology.ExpressApp.Editors.IModelCustomUserControlViewItem,
                Xafology.ExpressApp.Win.Editors.IModelWinCustomUserControlViewItem>();
        }
    }
}
