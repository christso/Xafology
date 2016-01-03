using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
namespace Xafology.ExpressApp.Xpo.Import
{
    public class ImportRequest : BaseObject, IImportRequest
    {
        public ImportRequest(DevExpress.Xpo.Session session)
            : base(session)
        {
            
        }
        
        private string requestLog;
        private string requestName;
        private string requestStatus;

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private DateTime requestTime;

        public DateTime RequestTime
        {
            get
            {
                return requestTime;
            }
            set
            {
                SetPropertyValue("RequestTime", ref requestTime, value);
            }
        }

        public string RequestLog
        {
            get
            {
                return requestLog;
            }
            set
            {
                SetPropertyValue("RequestLog", ref requestLog, value);
            }
        }

        public string RequestName
        {
            get
            {
                return requestName;
            }
            set
            {
                SetPropertyValue("RequestName", ref requestName, value);
            }
        }

        public string RequestStatus
        {
            get
            {
                return requestStatus;
            }
            set
            {
                SetPropertyValue("RequestStatus", ref requestStatus, value);
            }
        }


    }
}
