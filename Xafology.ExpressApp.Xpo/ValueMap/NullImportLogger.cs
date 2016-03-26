using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.ValueMap
{
    public class NullImportLogger : Xafology.ExpressApp.Xpo.ValueMap.IImportLogger
    {
        public void Log(string message, params object[] args)
        {
            return;
        }
    }
}
