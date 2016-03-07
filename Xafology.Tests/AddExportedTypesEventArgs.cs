using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.TestUtils
{
    public class AddExportedTypesEventArgs : EventArgs
    {
        private readonly ModuleBase module;

        public AddExportedTypesEventArgs(ModuleBase module)
        {
            this.module = module;
        }

        public ModuleBase Module
        {
            get
            {
                return module;
            }
        }
    }
}
