using Xafology.ExpressApp.PivotGridLayout;
using DevExpress.ExpressApp;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.PivotGridLayout
{
    public delegate void LayoutFieldsHandler(string layoutName);
    //public delegate void UILayoutFieldsHandler(object sender, string layoutName);
    public delegate object MapPivotGridFieldToUIHandler(PivotGridFieldBase baseField);

    public class PivotGridLayout
    {
        private string _Name;
        private LayoutFieldsHandler _LayoutFieldsByName;

        public PivotGridLayout(string name, LayoutFieldsHandler layoutFields)
        {
            Name = name;
            _LayoutFieldsByName = layoutFields;
        }

        public string Name { get { return _Name; } set { _Name = value; } }

        public void LayoutFields()
        {
            LayoutFieldsByName(Name);
        }

        public LayoutFieldsHandler LayoutFieldsByName
        {
            get
            {
                return _LayoutFieldsByName;
            }
        }
    }
}
