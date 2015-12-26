using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Concurrency
{
    public interface IRequestManager
    {
        void SubmitRequest(Action job, IActionRequest requestObj);
    }
}
