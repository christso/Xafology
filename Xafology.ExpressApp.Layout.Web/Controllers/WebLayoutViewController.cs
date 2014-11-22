using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Layout;
using System;

namespace Xafology.ExpressApp.Layout.Module.Web.Controllers
{
    public class WebLayoutViewController : ViewController<DetailView>
    {
        private void View_ControlsCreating(object sender, EventArgs e)
        {
            UpdateLayoutManagerTemplates();
        }
        private void UpdateLayoutManagerTemplates()
        {
            WebLayoutManager layoutManager = (WebLayoutManager)((DetailView)View).LayoutManager;

            // need to comment out one of the below otherwise you get an error
            // in a detail view that has a file attachment field
            //layoutManager.LayoutItemTemplate = new LayoutItemTemplateEx();
            layoutManager.LayoutGroupTemplate = new LayoutGroupTemplateEx();

            //LayoutBaseTemplate tabbedGroupTemplate1 = new TabbedGroupTemplateEx();
            //layoutManager.TabbedGroupTemplate = tabbedGroupTemplate1;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            View.ControlsCreating += new EventHandler(View_ControlsCreating);
        }
        protected override void OnDeactivated()
        {
            View.ControlsCreating -= new EventHandler(View_ControlsCreating);
            base.OnDeactivated();
        }
    }
}
