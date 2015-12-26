using DevExpress.ExpressApp.Security;
using System;
using System.Threading;
namespace Xafology.ExpressApp.Concurrency
{
    public interface IActionRequest
    {
        bool CancelRequest();
        DateTime RequestDate { get; set; }
        string RequestId { get; }
        string RequestLog { get; set; }
        string RequestName { get; set; }
        ISecurityUser Requestor { get; set; }
        RequestStatus RequestStatus { get; set; }
        void CommitChanges();
        void SetCancellationTokenSource(CancellationTokenSource cts);
    }
}
