using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Xafology.ExpressApp.PivotGridLayout.Win.Controllers
{
    // Hide Toolbar
    public class ToolbarViewController : ViewController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id == Data.PivotGridLayoutDashboardViewId
                || View.Id == Data.PivotGridSavedLayoutUISaveListViewId
                || View.Id == Data.PivotGridSavedLayoutUILoadListViewId)
            {
                Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.ToolbarVisibilityController>().SetToolbarVisibility(false);
                Frame.TemplateChanged += Frame_TemplateChanged;
            }
        }

        private NestedFrameTemplate NestedFrameTemplate
        {
            get { return Frame.Template as NestedFrameTemplate; }
        }
        void Frame_TemplateChanged(object sender, EventArgs e)
        {

            if (NestedFrameTemplate != null)
            {
                NestedFrameTemplate.ToolBar.Visible = false;
                NestedFrameTemplate.ToolBar.VisibleChanged += new EventHandler(ToolBar_VisibleChanged);
            }
        }
        void ToolBar_VisibleChanged(object sender, EventArgs e)
        {
            if (NestedFrameTemplate.ToolBar.Visible)
                NestedFrameTemplate.ToolBar.Visible = false;
        }
    }
}
