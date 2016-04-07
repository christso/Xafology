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
        private const string managedCaption = "Managed";
        private const string hardCaption = "Hard";
        private const string collectCaption = "Collect";

        private SingleChoiceAction batchDeleteAction;

        public BatchDeleteListViewControllerBase()
        {
            TargetViewType = ViewType.ListView;
            batchDeleteAction = new SingleChoiceAction(this, "BatchDeleteAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            batchDeleteAction.Caption = "Batch Delete";
            batchDeleteAction.ConfirmationMessage = "Do you want to delete all visible objects in this view?";
            batchDeleteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            batchDeleteAction.Execute += batchDeleteAction_Execute;

            var deleteChoice = new ChoiceActionItem();
            deleteChoice.Caption = managedCaption;
            batchDeleteAction.Items.Add(deleteChoice);

            var hardDeleteChoice = new ChoiceActionItem();
            hardDeleteChoice.Caption = hardCaption;
            batchDeleteAction.Items.Add(hardDeleteChoice);

            var collectDeleteChoice = new ChoiceActionItem();
            collectDeleteChoice.Caption = collectCaption;
            batchDeleteAction.Items.Add(collectDeleteChoice);
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
                case managedCaption:
                    ExecuteManagedDelete(objSpace, currentType, criteria);
                    break;
                case hardCaption:
                    ExecuteHardDelete(objSpace, criteria, classInfo);
                    break;
                case collectCaption:
                    ExecuteCollectDelete(objSpace, criteria, classInfo);
                    break;
            }
        }

        private void ExecuteManagedDelete(IObjectSpace objSpace, Type currentType, CriteriaOperator criteria)
        {
            var objs = objSpace.GetObjects(currentType, criteria);
            objSpace.Delete(objs);
            objSpace.CommitChanges();
        }

        private void ExecuteHardDelete(XPObjectSpace objSpace, CriteriaOperator criteria, XPClassInfo classInfo)
        {
            var sqlWhere = CriteriaToWhereClauseHelper.GetMsSqlWhere(XpoCriteriaFixer.Fix(criteria));
            sqlWhere = string.IsNullOrEmpty(sqlWhere) ? "" : " WHERE " + sqlWhere;
            var sqlNonQuery = "DELETE FROM " + classInfo.TableName
                + sqlWhere;
            objSpace.Session.ExecuteNonQuery(sqlNonQuery);
        }

        private void ExecuteCollectDelete(XPObjectSpace objSpace, CriteriaOperator criteria, XPClassInfo classInfo)
        {
            var gcRecordIDGenerator = new Random();
            var randomNumber = gcRecordIDGenerator.Next(1, 2147483647);
            var sqlWhere = CriteriaToWhereClauseHelper.GetMsSqlWhere(XpoCriteriaFixer.Fix(criteria));
            sqlWhere = string.IsNullOrEmpty(sqlWhere) ? "" : " WHERE " + sqlWhere;
            var sqlNonQuery = "UPDATE " + classInfo.TableName + " SET GCRecord = "
                + randomNumber
                + sqlWhere;
            objSpace.Session.ExecuteNonQuery(sqlNonQuery);
        }

        protected virtual CriteriaOperator ActiveFilterCriteria { get { return null; } }
        protected virtual bool ActiveFilterEnabled { get { return false; } }
    }
}
