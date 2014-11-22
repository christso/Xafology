using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xafology.ExpressApp.Editors;

namespace Xafology.CustomUserControlDemo.Win.Controls
{
    public partial class WinCustomUserControl : DevExpress.XtraEditors.XtraUserControl, IXpoSessionAwareControl
    {
        public WinCustomUserControl()
        {
            InitializeComponent();
        }

        void IXpoSessionAwareControl.UpdateDataSource(DevExpress.Xpo.Session session)
        {
            
        }
    }
}
