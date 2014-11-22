using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Templates;

namespace PivotGridLayoutDemo.Web
{
    public partial class DialogTemplateContent1 : TemplateContent, ILookupPopupFrameTemplate
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WebWindow window = WebWindow.CurrentRequestWindow;
            if (window != null)
            {
                string clientScript = string.Format(@" 
                    var activePopupControl = GetActivePopupControl(window.parent);
                    if (activePopupControl != null){{
                        var viewImageControl = document.getElementById('{0}');
                        if (viewImageControl && viewImageControl.src != ''){{
                            activePopupControl.SetHeaderImageUrl(viewImageControl.src);
                        }}
                        var viewCaptionControl = document.getElementById('{1}');
                        if (viewCaptionControl){{
                            var headerText = viewCaptionControl.innerText;
                            if(__aspxFirefox && viewCaptionControl.textContent) {{
                                headerText = viewCaptionControl.textContent;
                            }}
                            activePopupControl.SetHeaderText(headerText);
                        }}
                    }}", VIC.Control.ClientID, VCC.Control.ClientID);
                window.RegisterStartupScript("UpdatePopupControlHeader", clientScript, true);
                window.PagePreRender += new EventHandler(window_PagePreRender);
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(window_PagePreRender);
            }
            base.OnUnload(e);
        }
        private void window_PagePreRender(object sender, EventArgs e)
        {
            if ((SAC.HasActiveActions() && IsSearchEnabled) || OCC.HasActiveActions())
            {
                TableCell1.Style["padding-bottom"] = "30px";
            }
        }
        #region ILookupPopupFrameTemplate Members

        public bool IsSearchEnabled
        {
            get { return SAC.Visible; }
            set { SAC.Visible = value; }
        }

        public void SetStartSearchString(string searchString) { }

        #endregion
        #region IFrameTemplate Members

        public ICollection<DevExpress.ExpressApp.Templates.IActionContainer> GetContainers()
        {
            return null;
        }
        public void SetView(DevExpress.ExpressApp.View view)
        {
        }
        #endregion
        public override object ViewSiteControl
        {
            get
            {
                return VSC;
            }
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {
        }
        public override IActionContainer DefaultContainer
        {
            get { return null; }
        }
        public void FocusFindEditor() { }
    }
}
