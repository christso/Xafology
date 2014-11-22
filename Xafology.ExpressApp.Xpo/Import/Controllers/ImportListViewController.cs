using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using System;

namespace Xafology.ExpressApp.Xpo.Import.Controllers
{
    public abstract class ImportListViewController : ViewController<ListView>
    {
        protected SingleChoiceAction ImportDataAction;
        public Type ParamType;

        public ImportListViewController()
        {
            ImportDataAction = new SingleChoiceAction(this, "D2NImportData", PredefinedCategory.ObjectsCreation);
            ImportDataAction.Caption = "Import";
            ImportDataAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ImportDataAction.Execute += ImportDataActionExecute;

            ChoiceActionItem defaultActionItem = new ChoiceActionItem();
            defaultActionItem.Caption = "Default";
            ImportDataAction.Items.Add(defaultActionItem);

            ChoiceActionItem csvNameActionItem = new ChoiceActionItem();
            csvNameActionItem.Caption = "CSV-Head";
            ImportDataAction.Items.Add(csvNameActionItem);

            ChoiceActionItem csvOrdActionItem = new ChoiceActionItem();
            csvOrdActionItem.Caption = "CSV-Ord";
            ImportDataAction.Items.Add(csvOrdActionItem);
        }

        protected virtual void ImportDataActionExecute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

            if (e.SelectedChoiceActionItem.Caption == "CSV-Head")
                ParamType = typeof(ImportCsvFileHeadersParam);
            else if (e.SelectedChoiceActionItem.Caption == "CSV-Ord")
                ParamType = typeof(ImportCsvFileOrdinalsParam);
            else
                ParamType = typeof(ImportCsvFileHeadersParam);

            XPObjectSpace objSpace = (XPObjectSpace)Application.CreateObjectSpace();
            string viewId = Application.GetListViewId(ParamType);
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objSpace, ParamType, viewId);
            collectionSource.Criteria["ObjectTypeFilter"] = ImportCsvFileParamBase.Fields.ObjectTypeName == View.ObjectTypeInfo.Name;
            ListView listView = Application.CreateListView(viewId, collectionSource, true);

            ShowViewParameters svp = new ShowViewParameters(listView);
            svp.TargetWindow = TargetWindow.NewModalWindow;

            var dc = new DialogController();
            svp.Controllers.Add(dc);

            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
        }

    }
}
