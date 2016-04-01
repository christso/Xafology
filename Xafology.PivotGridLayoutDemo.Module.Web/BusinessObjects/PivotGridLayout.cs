using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.PivotGridLayoutDemo.Module.Web.BusinessObjects
{
    [DefaultClassOptions]
    public class PivotGridLayout : BaseObject
    {
        public PivotGridLayout(Session session) : base(session)
        {

        }

        private string _LayoutName;
        private string _TypeName;

        public string LayoutName
        {
            get
            {
                return _LayoutName;
            }
            set
            {
                SetPropertyValue("LayoutName", ref _LayoutName, value);
            }
        }
        
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                SetPropertyValue("TypeName", ref _TypeName, value);
            }
        }
    }
}
