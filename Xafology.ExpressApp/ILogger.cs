using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
    }
}
