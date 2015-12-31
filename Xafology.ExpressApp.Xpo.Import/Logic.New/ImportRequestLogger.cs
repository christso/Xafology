using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Concurrency;

namespace Xafology.ExpressApp.Xpo.Import.Logic.New
{
    public class ImportRequestLogger : IImportLogger
    {
        private readonly ActionRequest request;

        public ImportRequestLogger(ActionRequest request)
        {
            this.request = request;
        }

        public void Log(string message, params object[] args)
        {
            if (request == null) return;
            if (!string.IsNullOrEmpty(request.RequestLog))
                request.RequestLog += "\r\n";
            request.RequestLog += string.Format(message, args);
        }
    }
}
