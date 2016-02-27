using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout
{
    public class ReservedLayoutNames : List<ReservedLayoutName>
    {
        public void Add(string name, string typeName)
        {
            var reserved = new ReservedLayoutName(name, typeName);
            if (!this.Contains(reserved))
                base.Add(new ReservedLayoutName(name, typeName));
        }
    }
}
