using Xafology.ExpressApp.Xpo.Import;
using Xafology.ImportDemo.Module.BusinessObjects;
using Xafology.ImportDemo.Module.ParamObjects;

namespace Xafology.ImportDemo.Module.Controllers
{
    public class MyImportViewController : Xafology.ExpressApp.Xpo.Import.Controllers.ImportListViewController
    {
        public MyImportViewController()
        {
        }
        protected override void ImportDataActionExecute(object sender, DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs e)
        {
            if (View.ObjectTypeInfo.Type == typeof(MyLookupObject) && e.SelectedChoiceActionItem.Caption == "Default")
            {
                var paramObj = new ImportForexRatesParam();
                var dialog = new Xafology.ExpressApp.SystemModule.PopupDialogDetailViewManager(Application);
                dialog.CanCloseWindow = false;
                dialog.ShowNonPersistentView(paramObj);
            }
            else
            {
                base.ImportDataActionExecute(sender, e);
            }
        }
    }
}
