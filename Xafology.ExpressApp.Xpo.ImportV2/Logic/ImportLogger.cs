using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class ImportLogger : IImportLogger
    {
        private readonly IImportRequest request;

        public ImportLogger(IImportRequest request)
        {
            this.request = request;
        }

        public void Log(string message, params object[] args)
        {
            //if (request == null) return;
            if (!string.IsNullOrEmpty(request.RequestLog))
                request.RequestLog += "\r\n";
            request.RequestLog += string.Format(message, args);
        }

        public void LogStatus(string message)
        {
            //if (request == null) return;
            
        }
    }
}
