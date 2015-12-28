using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.SystemModule;
using System.Threading;

namespace Xafology.ExpressApp.Concurrency
{
    public class LoggedRequestManager : IRequestManager
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }
        private readonly ActionRequestLogger logger;
        private ActionRequest request;
        
        // override the exit message
        public RequestStatus? CustomRequestExitStatus { get; set; }
        
        private readonly XafApplication application;

        public LoggedRequestManager(XafApplication application)
        {
            this.application = application;
            logger = new ActionRequestLogger(this);
        }

        public ActionRequest Request
        {
            get
            {
                return request;
            }
        }

        private static void SetRequester(ActionRequest request)
        {
            var currentUser = SecuritySystem.CurrentUser as SecuritySystemUser;
            if (currentUser != null)
            {
                request.Requestor = request.Session.GetObjectByKey<SecuritySystemUser>(currentUser.Oid);
            }
        }

        private void CreateRequest(string requestName, CancellationTokenSource ts)
        {
            var objSpace = application.CreateObjectSpace();
            request = objSpace.CreateObject<ActionRequest>();
            SetRequester(request);
            request.RequestName = requestName;
            request.RequestStatus = RequestStatus.Processing;
            request.CommitChanges();

            request.SetCancellationTokenSource(ts);
            request.CommitChanges();
        }

        public void ProcessRequest(string requestName, Action job)
        {
            var ts = new CancellationTokenSource();
            CancellationTokenSource = ts;

            CreateRequest(requestName, ts);
            new GenericMessageBox("Request ID: " + request.RequestId, "Concurrent Request");

            ProcessRequest(job, request);
        }

        private void ProcessRequest(Action job, ActionRequest requestObj)
        {
            CustomRequestExitStatus = null;
            request = requestObj;

            try
            {
                job();

                // use the user-defined exit status, otherwise assume request is complete
                if (CustomRequestExitStatus == null)
                    requestObj.RequestStatus = RequestStatus.Complete;
                else
                    requestObj.RequestStatus = (RequestStatus)CustomRequestExitStatus;
            }
            catch (Exception ex)
            {
                // log unhandled exception
                requestObj.RequestStatus = RequestStatus.Error;
                logger.Log("Unhandled Error: {0}\r\n{1}", ex.Message, ex.StackTrace);
            }
            requestObj.CommitChanges();
        }

        public void Log(string text, params object[] args)
        {
            logger.Log(text);
        }
    }
}
