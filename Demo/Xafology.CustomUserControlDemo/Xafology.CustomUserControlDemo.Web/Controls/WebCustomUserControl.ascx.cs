using Xafology.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;

namespace Xafology.CustomUserControlDemo.Web.Controls
{
    public partial class WebCustomUserControl : UserControl, IXpoSessionAwareControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void IXpoSessionAwareControl.UpdateDataSource(Session session)
        {
        }
    }
}