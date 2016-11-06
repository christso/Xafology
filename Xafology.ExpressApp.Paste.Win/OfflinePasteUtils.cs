using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Paste.Parameters;
using Xafology.ExpressApp.Xpo;
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Xpo.ValueMap;

namespace Xafology.ExpressApp.Paste.Win
{
    public class OfflinePasteUtils
    {
        private readonly IXpoFieldValueReader xpoFieldValueReader;
        private readonly XpoFieldMapper xpoFieldMapper;
        private SimpleImportLogger logger;

        public OfflinePasteUtils(IXpoFieldValueReader xpoFieldReader)
        {
            this.xpoFieldValueReader = xpoFieldReader;
            this.logger = new SimpleImportLogger();
            this.xpoFieldMapper = new XpoFieldMapper(logger);
        }

        public OfflinePasteUtils()
        {
            xpoFieldValueReader = new XpoFieldValueReader();
            this.logger = new SimpleImportLogger();
            this.xpoFieldMapper = new XpoFieldMapper(logger);
        }

        public void PasteColumnsToRow(string[] copiedRowValues, IXPObject obj, ListView view)
        {
            PasteColumnsToRow(copiedRowValues, obj, view, null);
        }

        public SimpleImportLogger Logger
        {
            get { return logger; }
        }

        public void PasteColumnsToRow(string[] copiedRowValues, IXPObject obj, ListView view, PasteParam pasteParam, int startColumnIndex)
        {
            var listEditor = ((ListView)view).Editor as GridListEditor;
            var objSpace = ((ListView)view).ObjectSpace;
            var gridView = listEditor.GridView;

            // iterate through columns in listview
            int gridColumnIndex = startColumnIndex;
            int copiedColumnIndex = 0;

            var hasGroups = gridView.Columns.Where(x => x.GroupIndex != -1).Count() > 0;
            if (hasGroups)
                throw new UserFriendlyException("Please ungroup columns before pasting rows");

            var gridColumns = gridView.Columns
                .Where(x => x.Visible && x.GroupIndex == -1
                    && x.VisibleIndex >= startColumnIndex)
                .OrderBy(x => x.VisibleIndex)
                .Select(x => x);

            foreach (var gridColumn in gridColumns)
            {
                if (copiedColumnIndex >= copiedRowValues.Length)
                    break;

                var member = listEditor.Model.Columns[gridColumn.Name];
                var isLookup = typeof(IXPObject).IsAssignableFrom(member.ModelMember.MemberInfo.MemberType);

                var copiedValue = copiedRowValues[copiedColumnIndex];

                // if column is visible in grid, then increment the copiedValue column counter
                if (gridColumn.Visible)
                {
                    gridColumnIndex++;
                    copiedColumnIndex++;
                }
                // skip non-editable, key column, invisible column or blank values
                // otherwise paste values

                var memberInfo = member.ModelMember.MemberInfo;
                if (member.AllowEdit
                    && member.PropertyName != listEditor.Model.ModelClass.KeyProperty
                    && !string.IsNullOrEmpty(copiedValue)
                    && !string.IsNullOrWhiteSpace(copiedValue)
                    )
                {
                    PasteFieldMap fieldMap = null;
                    if (pasteParam != null)
                        fieldMap = pasteParam.FieldMaps.Where(m => m.TargetName.ToLower() == gridColumn.Name.ToLower()).FirstOrDefault();

                    #region Core Logic
                    if (fieldMap == null)
                    {
                        xpoFieldMapper.SetMemberValue(obj, memberInfo, copiedValue, true, true);
                    }
                    else
                    {
                        xpoFieldMapper.SetMemberValue(obj, memberInfo, copiedValue,
                            fieldMap.CreateMember, fieldMap.CacheObject);
                    }
                    #endregion
                }
            }
        }

        public void PasteColumnsToRow(string[] copiedRowValues, IXPObject obj, ListView view, PasteParam pasteParam)
        {
            PasteColumnsToRow(copiedRowValues, obj, view, pasteParam, 0);
        }

        public void PasteColumn(string[] copiedValues, GridListEditor listEditor, ListView view)
        {
            var gridView = listEditor.GridView;
            var gridColumn = gridView.FocusedColumn;
            var columnKey = gridColumn.FieldName.Replace("!", string.Empty);
            var member = listEditor.Model.Columns[gridColumn.Name];
            var memberInfo = member.ModelMember.MemberInfo;

            if (!member.AllowEdit)
                throw new InvalidOperationException("Column '" + gridView.FocusedColumn.Caption + "' is not editable.");

            var objs = view.SelectedObjects;
            int copyIndex = 0;
            foreach (IXPObject obj in objs)
            {
                string copiedValue = copiedValues[copyIndex];

                // paste object to focused cell
                xpoFieldMapper.SetMemberValue(obj, memberInfo, copiedValue);

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
