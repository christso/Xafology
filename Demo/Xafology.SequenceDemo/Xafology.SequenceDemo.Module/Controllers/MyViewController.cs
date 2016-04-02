using Xafology.SequenceDemo.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.SequenceDemo.Module.Controllers
{
    public class MyViewController : ViewController
    {
        public MyViewController()
        {
            var testAction = new SingleChoiceAction(this, "TestAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            testAction.ShowItemsOnClick = true;
            testAction.Caption = "Test";
            testAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            testAction.Execute += testAction_Execute;

            var choice1 = new ChoiceActionItem();
            choice1.Caption = "Sequence";
            testAction.Items.Add(choice1);

            var choice2 = new ChoiceActionItem();
            choice2.Caption = "Time";
            testAction.Items.Add(choice2);
        }

        void testAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case "Sequence":
                    CreateUserFriendlyIdObject(Application.CreateObjectSpace());
                    break;
                case "Time":
                    CreateTimedObject(Application.CreateObjectSpace());
                    break;
            }
        }

        public void CreateUserFriendlyIdObject(IObjectSpace objSpace)
        {
            var obj1 = objSpace.CreateObject<UserFriendlyIdObject1>();
            var id = obj1.SequentialNumber;
            objSpace.CommitChanges();
        }

        public void CreateTimedObject(IObjectSpace objSpace)
        {
            var obj1 = objSpace.CreateObject<TimedObject1>();
            objSpace.CommitChanges();

            var obj2 = objSpace.CreateObject<TimedObject1>();
            objSpace.CommitChanges();
        }

    }
}
