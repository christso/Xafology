using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.ValueMap;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class SimpleImportLogger: IImportLogger
    {
        private string logMessage;

        public string LogMessage
        {
            get { return logMessage; }
        }

        public void Log(string message, params object[] args)
        {
            //if (request == null) return;
            if (!string.IsNullOrEmpty(logMessage))
                logMessage += "\r\n";
            logMessage += string.Format(message, args);
        }
    }
}
