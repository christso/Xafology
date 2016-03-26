using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo;
using Xafology.ExpressApp.Xpo.ValueMap;

namespace Xafology.ExpressApp.Paste.Win
{
    public class PasteUtils
    {
        private readonly IXpoFieldValueReader xpoFieldValueReader;

        public PasteUtils(IXpoFieldValueReader xpoFieldReader)
        {
            this.xpoFieldValueReader = xpoFieldReader;
        }

        public PasteUtils()
        {
            xpoFieldValueReader = new XpoFieldValueReader();
        }
    
        public void PasteColumnsToRow(string[] copiedRowValues, int focusedRowHandle, GridListEditor listEditor, IObjectSpace objSpace)
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

                var pasteValue = xpoFieldValueReader.GetMemberValue(((XPObjectSpace)objSpace).Session, 
                    member.MemberInfo, copiedValue, true, true);

                gridView.SetRowCellValue(focusedRowHandle, gridColumn, pasteValue);
            }
        }

        public void PasteColumn(string[] copiedValues, GridListEditor listEditor, IObjectSpace objSpace)
        {
            var gridView = listEditor.GridView;
            var gridColumn = gridView.FocusedColumn;
            var columnKey = gridColumn.FieldName.Replace("!", string.Empty);
            var member = listEditor.Model.ModelClass.OwnMembers[columnKey]; // TODO: remove ! suffice when column is a lookup

            if (!member.AllowEdit)
                throw new InvalidOperationException("Column '" + gridView.FocusedColumn.Caption + "' is not editable.");

            int[] selectedRows = gridView.GetSelectedRows();
            int copyIndex = 0;
            foreach (int rowHandle in selectedRows)
            {
                string copiedValue = copiedValues[copyIndex];

                var pasteValue = xpoFieldValueReader.GetMemberValue(((XPObjectSpace)objSpace).Session,
                    member.MemberInfo, copiedValue, true, true);

                // paste object to focused cell
                gridView.SetRowCellValue(rowHandle, gridView.FocusedColumn, pasteValue);

                if (copyIndex >= copiedValues.Length - 1)
                    // reset copied row counter to the beginning after the last row is reached
                    copyIndex = 0;
                else
                    // go to next copied value
                    copyIndex++;
            }
        }
    }
}
