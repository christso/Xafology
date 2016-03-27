using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo;

namespace Xafology.ExpressApp.BatchDelete
{
    public class BatchDeleteListViewControllerBase : ViewController
    {
        public BatchDeleteListViewControllerBase()
        {
            TargetViewType = ViewType.ListView;
            batchDeleteAction = new SingleChoiceAction(this, "BatchDeleteAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            batchDeleteAction.Caption = "Batch Delete";
            batchDeleteAction.ConfirmationMessage = "Do you want to delete all visible objects in this view?";
            batchDeleteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            batchDeleteAction.Execute += batchDeleteAction_Execute;

            var deleteAllActionItem = new ChoiceActionItem();
            deleteAllActionItem.Caption = "Delete Filtered";
            batchDeleteAction.Items.Add(deleteAllActionItem);

            var quickDeleteActionItem = new ChoiceActionItem();
            quickDeleteActionItem.Caption = "Quick Delete Filtered";
            batchDeleteAction.Items.Add(quickDeleteActionItem);
        }

        private void batchDeleteAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

            var objSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var currentTypeInfo = ((ObjectView)View).ObjectTypeInfo;
            var currentType = currentTypeInfo.Type;
            XPClassInfo classInfo = objSpace.Session.GetClassInfo(currentType);
            CriteriaOperator criteria = ActiveFilterEnabled ? ActiveFilterCriteria : null;

            switch (e.SelectedChoiceActionItem.Caption)
            {
                case "Delete Filtered":
                    var objs = objSpace.GetObjects(currentType, criteria);
                    objSpace.Delete(objs);
                    objSpace.CommitChanges();
                    break;
                case "Quick Delete Filtered":
                    var sqlWhere = CriteriaToWhereClauseHelper.GetMsSqlWhere(XpoCriteriaFixer.Fix(criteria));
                    sqlWhere = string.IsNullOrEmpty(sqlWhere) ? "" : " WHERE " + sqlWhere;
                    var sqlNonQuery = "DELETE FROM " + classInfo.TableName
                        + sqlWhere;
                    objSpace.Session.ExecuteNonQuery(sqlNonQuery);
                    break;
            }
        }
        private SingleChoiceAction batchDeleteAction;

        protected virtual CriteriaOperator ActiveFilterCriteria { get { return null; } }
        protected virtual bool ActiveFilterEnabled { get { return false; } }
    }
}
