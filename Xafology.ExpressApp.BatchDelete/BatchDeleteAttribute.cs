using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.BatchDelete
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class BatchDeleteAttribute : System.Attribute
    {
        private bool isVisible = true;

        public BatchDeleteAttribute()
        {
        }
        public BatchDeleteAttribute(bool isVisible)
        {
            this.isVisible = isVisible;
        }
        public bool IsVisible
        {
            get { return isVisible; }
        }
    }
}
