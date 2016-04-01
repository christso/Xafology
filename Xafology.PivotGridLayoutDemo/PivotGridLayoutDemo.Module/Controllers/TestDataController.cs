using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using PivotGridLayoutDemo.Module.DatabaseUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotGridLayoutDemo.Module.Controllers
{
    public class TestDataController : ViewController
    {
        public TestDataController()
        {
            var myAction = new SingleChoiceAction(this, "TestDataAction", PredefinedCategory.ObjectsCreation);
            myAction.Caption = "TestData";
            myAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            myAction.Execute += testDataAction_Execute;
            var myActionItem = new ChoiceActionItem();
            myActionItem.Caption = "Default";
            myAction.Items.Add(myActionItem);
        }

        private void testDataAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var data = new DataInitializer(ObjectSpace);
            data.Run();
            ObjectSpace.CommitChanges();
        }
    }
}
