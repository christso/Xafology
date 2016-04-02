using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.SystemModule;
using Xafology.ShowWindowDemo.Module.BusinessObjects;

namespace Xafology.ShowWindowDemo.Module.Controllers
{
    public class MyViewController : ViewController
    {
        public MyViewController()
        {
            var myAction = new SingleChoiceAction(this, "ShowWindowAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            myAction.Caption = "Show Window";
            myAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            myAction.ShowItemsOnClick = true;
            myAction.Execute += myAction_Execute;

            var choice1 = new ChoiceActionItem();
            choice1.Caption = "Singleton";
            myAction.Items.Add(choice1);
        }

        void myAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case "Singleton":
                    var dialog = new PopupDialogDetailViewManager(Application);
                    dialog.ShowSingletonView<SingletonObject1>();
                    break;
            }
        }
    }
}
