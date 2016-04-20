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
        private bool isOptimized = true;

        public BatchDeleteAttribute()
        {
        }
        public BatchDeleteAttribute(bool isVisible, bool isOptimized)
        {
            this.isVisible = isVisible;
            this.isOptimized = true;
        }
        public bool IsVisible
        {
            get { return isVisible; }
        }

        public bool IsOptimized
        {
            get { return isOptimized; }
        }
    }
}
