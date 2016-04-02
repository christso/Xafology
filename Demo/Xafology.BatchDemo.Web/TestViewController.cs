using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.BatchDemo.Win
{
    public class TestViewController : ViewController
    {
        public TestViewController()
        {
            var myAction = new SingleChoiceAction(this, "MyAction", PredefinedCategory.ObjectsCreation);
            myAction.Caption = "MyAction";
            myAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            myAction.Execute += myAction_Execute;
            var myActionItem = new ChoiceActionItem();
            myActionItem.Caption = "Default";
            myAction.Items.Add(myActionItem);
        }

        private void myAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            
        }
    }
}
