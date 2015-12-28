using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Concurrency
{
    public interface IRequestManager
    {
        CancellationTokenSource CancellationTokenSource { get; set; }
        void ProcessRequest(string requestName, Action job);
        void Log(string text, params object[] args);
        RequestStatus? CustomRequestExitStatus { get; set; }
        ActionRequest Request { get; }
    }
}
