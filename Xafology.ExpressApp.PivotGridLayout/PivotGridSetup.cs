using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout
{
    public class PivotGridSetup
    {
        public PivotGridSetup()
        {
            _Fields = new List<PivotGridFieldBase>();
            _Layouts = new List<PivotGridLayout>();

        }

        private List<PivotGridLayout> _Layouts;
        private List<PivotGridFieldBase> _Fields;

        public List<PivotGridFieldBase> Fields
        {
            get
            {
                return _Fields;
            }
        }

        public List<PivotGridLayout> Layouts
        {
            get
            {
                return _Layouts;
            }
        }
    }
}
