using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp;
using Xafology.ExpressApp.SystemModule;
using System.Threading;

namespace Xafology.ExpressApp.Concurrency
{
    public class RequestManager
    {
        public CancellationTokenSource CancellationTokenSource;
        private ActionRequest _RequestObj;
        public RequestStatus? CustomRequestExitStatus;
        public readonly XafApplication Application;

        public RequestManager(XafApplication application)
        {
            this.Application = application;
        }

        public void SubmitRequest(string requestName, Action job)
        {
            var ts = new CancellationTokenSource();
            CancellationTokenSource = ts;

            var currentUser = SecuritySystem.CurrentUser as SecuritySystemUser;
            var objSpace = Application.CreateObjectSpace();
            var requestObj = objSpace.CreateObject<ActionRequest>();
            if (currentUser != null)
                requestObj.Requestor = objSpace.GetObjectByKey<SecuritySystemUser>(currentUser.Oid);
            requestObj.RequestName = requestName;
            requestObj.RequestStatus = RequestStatus.Processing;
            requestObj.Save();
            requestObj.Session.CommitTransaction();

            requestObj.SetCancellationTokenSource(ts);
            requestObj.Save();
            requestObj.Session.CommitTransaction();
            new GenericMessageBox("Request ID: " + requestObj.RequestId, "Concurrent Request");

            Task t = Task.Factory.StartNew(() =>
            {
                SubmitRequest(job, requestObj);
            }, ts.Token);
        }

        public void WriteLogLine(string text, bool commit = true)
        {
            if (_RequestObj == null) return;
            if (!string.IsNullOrEmpty(_RequestObj.RequestLog))
                _RequestObj.RequestLog += "\r\n";
            _RequestObj.RequestLog += text;
            if (commit)
            {
                _RequestObj.Save();
                _RequestObj.Session.CommitTransaction();
            }
        }

        protected void SubmitRequest(Action job, ActionRequest requestObj)
        {
            CustomRequestExitStatus = null;
            _RequestObj = requestObj;

            try
            {
                job();
                if (CustomRequestExitStatus == null)
                    requestObj.RequestStatus = RequestStatus.Complete;
                else
                    requestObj.RequestStatus = (RequestStatus)CustomRequestExitStatus;
            }
            catch (Exception ex)
            {
                requestObj.RequestStatus = RequestStatus.Error;
                WriteLogLine("Unhandled Error: " + ex.Message + "\r\n" + ex.StackTrace);
            }
            requestObj.Save();
            requestObj.Session.CommitTransaction();
        }
    }
}
