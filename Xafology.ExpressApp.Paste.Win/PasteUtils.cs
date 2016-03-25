using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Win
{
    public class PasteUtils
    {
        public static void PasteColumnsToRow(string[] copiedRowValues, int focusedRowHandle, GridListEditor listEditor, IObjectSpace objSpace)
        {
            var gridView = listEditor.GridView;

            // iterate through columns in listview
            for (int columnIndex = 0; columnIndex < listEditor.Model.ModelClass.OwnMembers.Count; columnIndex++)
            {
                var member = listEditor.Model.ModelClass.OwnMembers[columnIndex];
                var isLookup = !string.IsNullOrEmpty(member.LookupProperty);
                var gridColumnKey = member.Name + (isLookup ? "!" : "");
                var gridColumn = gridView.Columns[gridColumnKey];
                var copiedValue = copiedRowValues[columnIndex];
                
                // skip non-editable, key or invisible column
                if (!member.AllowEdit|| member.Name == listEditor.Model.ModelClass.KeyProperty 
                    || !gridColumn.Visible)
                    continue;

                object pasteValue = null;
                if (isLookup)
                {
                    var objType = gridColumn.ColumnType;
                    var criteria = CriteriaOperator.Parse(string.Format(
                        "{0} LIKE ?", member.LookupProperty), copiedValue);
                    pasteValue = objSpace.FindObject(objType, criteria);
                }
                else
                {
                    pasteValue = copiedValue;
                }

                gridView.SetRowCellValue(focusedRowHandle, gridColumn, pasteValue);
            }
        }
    }
}
