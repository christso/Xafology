#if DEBUG
using Xafology.ExpressApp.MsoExcel.Reports;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;

namespace Xafology.ExpressApp.MsoExcel.Controllers
{
    public class TestController : ViewController
    {
        public TestController()
        {
            var testAction = new SimpleAction(this, "TestAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            testAction.Caption = "Test";
            testAction.Execute += testAction_Execute;
        }

        void testAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //Setup.InitializeReportCreators(((XPObjectSpace)ObjectSpace).Session);
            Setup.CreateReportObjects(((XPObjectSpace)ObjectSpace).Session);
        }
    }
}
#endif