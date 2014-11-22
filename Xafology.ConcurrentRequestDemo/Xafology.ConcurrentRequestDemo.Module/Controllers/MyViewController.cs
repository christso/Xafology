using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System.Threading;

namespace Xafology.ConcurrentRequestDemo.Module.Controllers
{
    public class MyViewController : ViewController
    {
        public MyViewController()
        {
            var myAction = new SimpleAction(this, "MyAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            myAction.Execute += myAction_Execute;
        }

        void myAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var request = new Xafology.ExpressApp.Concurrency.RequestManager(Application);
            request.SubmitRequest("Job 1", Job1);
        }

        public void Job1()
        {
            Thread.Sleep(6000);
        }
    }
}
