using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
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

    }
}
