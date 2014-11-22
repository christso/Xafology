using Xafology.ExpressApp.Concurrency;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Concurrency.Controllers
{
    public class ActionRequestViewController : ViewController
    {
        public ActionRequestViewController()
        {
            TargetObjectType = typeof(ActionRequest);

            SimpleAction cancelAction = new SimpleAction(this, "CancelActionRequestAction", "ActionRequestDetailViewActions");
            cancelAction.Caption = "Cancel";
            cancelAction.Execute += cancelAction_Execute;
        }

        void cancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var requestObj = (ActionRequest)View.CurrentObject;
            if (!requestObj.CancelRequest()) return;
            requestObj.RequestStatus = RequestStatus.Cancelled;
            requestObj.Save();
            ObjectSpace.CommitChanges();
        }
    }
}
