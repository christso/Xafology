using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using PivotGridLayoutDemo.Module.BusinessObjects;
using PivotGridLayoutDemo.Module.DatabaseUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotGridLayoutDemo.Module.Controllers
{
    public class DemoWelcomeController : ObjectViewController<DetailView, DemoWelcome>
    {
        public DemoWelcomeController()
        {
            var createTestDataAction = new SimpleAction(this, "CreateTestDataAction", "WelcomeActionContainer");
            createTestDataAction.Caption = "Create Test Data";
            createTestDataAction.Execute += testDataAction_Execute;
        }

        private void testDataAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var data = new DataInitializer(ObjectSpace);
            data.Run();
            ObjectSpace.CommitChanges();
        }
    }
}
