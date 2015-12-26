using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Actions;
using System.Threading;
using Xafology.ExpressApp.Xpo.SequentialBase;

namespace Xafology.ExpressApp.Concurrency
{
    public class ActionRequest : SequentialBaseObject
    {
        public ActionRequest(Session session)
            : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code (check out http://documentation.devexpress.com/#Xaf/CustomDocument2834 for more details).

            //// Cannot get currentUser on a separate thread in ASP.NET
            //SecuritySystemUser currentUser = GlobalHelper.GetCurrentUser(Session);
            //if (currentUser != null)
            //    this.Requestor = currentUser;

            RequestDate = DateTime.Now;
        }

        #region Fields
        private string _RequestLog;
        private string _RequestName;
        private RequestStatus _RequestStatus;
        private DateTime _RequestDate;
        SecuritySystemUser _Requestor;

        [PersistentAlias("concat('RQ', ToStr(SequentialNumber))")]
        public string RequestId
        {
            get
            {
                return Convert.ToString(EvaluateAlias("RequestId"));
            }
        }
        #endregion

        #region Methods

        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public static Dictionary<Guid, CancellationTokenSource> CancellationTokenSources = new Dictionary<Guid, CancellationTokenSource>();

        public void SetCancellationTokenSource(CancellationTokenSource cts)
        {
            if (!CancellationTokenSources.ContainsKey(Oid))
                CancellationTokenSources.Add(Oid, cts);
            else
                CancellationTokenSources[Oid] = cts;
        }

        public bool CancelRequest()
        {
            if (!CancellationTokenSources.ContainsKey(Oid)) return false;
            CancellationTokenSources[Oid].Cancel();
            CancellationTokenSources[Oid].Dispose();
            CancellationTokenSources.Remove(Oid);
            return true;
        }

        #endregion

        #region Properties

        public DateTime RequestDate
        {
            get
            {
                return _RequestDate;
            }
            set
            {
                SetPropertyValue("RequestDate", ref _RequestDate, value);
            }
        }
        public string RequestName
        {
            get
            {
                return _RequestName;
            }
            set
            {
                SetPropertyValue("RequestName", ref _RequestName, value);
            }
        }
        public SecuritySystemUser Requestor
        {
            get
            {
                return _Requestor;
            }
            set
            {
                SetPropertyValue("Requestor", ref _Requestor, value);
            }
        }

        public RequestStatus RequestStatus
        {
            get
            {
                return _RequestStatus;
            }
            set
            {
                SetPropertyValue("RequestStatus", ref _RequestStatus, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string RequestLog
        {
            get
            {
                return _RequestLog;
            }
            set
            {
                SetPropertyValue("RequestLog", ref _RequestLog, value);
            }
        }


        #endregion

        public void CommitChanges()
        {
            this.Save();
            this.Session.CommitTransaction();
        }
    }
}
