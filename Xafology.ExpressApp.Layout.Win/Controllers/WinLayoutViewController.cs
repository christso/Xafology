using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using System;

namespace Xafology.ExpressApp.Layout.Module.Win.Controllers
{
    public class WinLayoutViewController : ViewController<DetailView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.ControlsCreated += View_ControlsCreated;
        }

        void View_ControlsCreated(object sender, EventArgs e)
        {
            LayoutControl layoutControl = ((LayoutControl)((DetailView)View).Control);
            layoutControl.BeginUpdate();
            try
            {
                foreach (object item in layoutControl.Items)
                {
                    if (item is LayoutControlGroup)
                    {
                        ((LayoutControlGroup)item).ExpandButtonVisible = true;
                        ((LayoutControlGroup)item).Expanded = true;
                        ((LayoutControlGroup)item).HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
                        ((LayoutControlGroup)item).ExpandOnDoubleClick = true;
                    }
                }
            }
            finally
            {
                layoutControl.EndUpdate();
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            View.ControlsCreated -= View_ControlsCreated;
        }
    }
}
