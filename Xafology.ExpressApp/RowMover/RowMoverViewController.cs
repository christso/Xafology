using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.SystemModule;

namespace Xafology.ExpressApp.RowMover
{
    public class RowMoverViewController : ObjectViewController<ListView, Xafology.ExpressApp.RowMover.IRowMoverObject>
    {
        private Xafology.ExpressApp.RowMover.RowMover mover;

        public RowMoverViewController()
        {
            var moveAction = new SingleChoiceAction(this, "MappingMoveAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            moveAction.Caption = "Move";
            moveAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            moveAction.Execute += moveAction_Execute;

            var moveUpActionItem = new ChoiceActionItem();
            moveUpActionItem.Caption = "Up";
            moveAction.Items.Add(moveUpActionItem);

            var moveDownActionItem = new ChoiceActionItem();
            moveDownActionItem.Caption = "Down";
            moveAction.Items.Add(moveDownActionItem);

            var moveTopActionItem = new ChoiceActionItem();
            moveTopActionItem.Caption = "Top";
            moveAction.Items.Add(moveTopActionItem);

            var moveBottomActionItem = new ChoiceActionItem();
            moveBottomActionItem.Caption = "Bottom";
            moveAction.Items.Add(moveBottomActionItem);

            var moveCustomActionItem = new ChoiceActionItem();
            moveCustomActionItem.Caption = "Custom";
            moveAction.Items.Add(moveCustomActionItem);

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            mover = new Xafology.ExpressApp.RowMover.RowMover(ObjectSpace);
        }

        private void moveAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var obj = e.CurrentObject as Xafology.ExpressApp.RowMover.IRowMoverObject;
            if (obj == null) return;

            switch (e.SelectedChoiceActionItem.Caption)
            {
                case "Up":
                    mover.MoveUp(obj);
                    break;
                case "Down":
                    mover.MoveDown(obj);
                    break;
                case "Top":
                    mover.MoveToTop(obj);
                    break;
                case "Bottom":
                    mover.MoveToBottom(obj);
                    break;
                case "Custom":
                    var popupView = new Xafology.ExpressApp.SystemModule.PopupDialogDetailViewManager(Application);
                    popupView.Accepting += DialogController_Accepting;
                    popupView.ShowNonPersistentView(typeof(MoveToRowIndexParam));
                    break;

            }
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var paramObj = (MoveToRowIndexParam)e.AcceptActionArgs.CurrentObject;
            mover.MoveTo((IRowMoverObject)View.CurrentObject, paramObj.Index);
        }
    }
}
