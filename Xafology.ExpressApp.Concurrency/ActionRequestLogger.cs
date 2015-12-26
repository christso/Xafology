using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Concurrency
{
    public class ActionRequestLogger : ILogger
    {
        private readonly bool immediate;
        private readonly RequestManager manager;

        public ActionRequestLogger(RequestManager manager, bool immediate = true)
        {
            this.manager = manager;
            this.immediate = immediate;

            if (manager == null)
                throw new ArgumentNullException("Argument 'manager' cannot be null.");
        }

        public void Log(string message, params object[] args)
        {
            var request = manager.Request;

            if (request == null) return;
            if (!string.IsNullOrEmpty(request.RequestLog))
                request.RequestLog += "\r\n";
            request.RequestLog += string.Format(message, args);
            if (immediate)
            {
                request.CommitChanges();
            }
        }
    }
}
