using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public interface IImportLogger
    {
        void Log(string message, params object[] args);
    }
}
