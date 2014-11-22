using DevExpress.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Editors
{
    public delegate void ActionPropertyClickEventHandler(PropertyEditor sender, ActionPropertyClickEventArgs e);

    public class ActionPropertyClickEventArgs
    {
        public object Value { get; set; }
    }

    public interface IActionPropertyEditor
    {
        event ActionPropertyClickEventHandler ButtonClick;
    }
}
