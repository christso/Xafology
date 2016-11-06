using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Paste.Parameters;

namespace Xafology.ExpressApp.Paste.Win
{
    public class ExistingRowPasteProcessor
    {
        private readonly ICopyParser copyParser;
        private readonly View view;
        private readonly PasteUtils pasteUtils;
        private readonly OfflinePasteUtils offlinePasteUtils;

        public ExistingRowPasteProcessor(ICopyParser copyParser, View view)
        {
            this.copyParser = copyParser;
            this.view = view;
            this.pasteUtils = new PasteUtils();
            this.offlinePasteUtils = new OfflinePasteUtils();
        }
        public Xpo.ValueMap.IImportLogger Logger
        {
            get { return offlinePasteUtils.Logger; }
        }

        // note that the new row must be focused for this to work
        public void Process(int[] selectedRowHandles, PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();

            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // add new row in gridview
                gridView.FocusedRowHandle = selectedRowHandles[r];

                // paste cells
                pasteUtils.PasteColumnsToRow(copiedValues[r], gridView.FocusedRowHandle,
                    listview, pasteParam);

                gridView.UpdateCurrentRow();
            }
        }

        public void Process(PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            int[] selectedRowHandles = gridView.GetSelectedRows();
            var gridColumn = gridView.FocusedColumn;
            var copiedValues = copyParser.ToArray();

            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // select next row in gridview
                gridView.FocusedRowHandle = selectedRowHandles[r];

                // paste cells
                pasteUtils.PasteColumnsToRow(copiedValues[r], gridView.FocusedRowHandle,
                    listview, pasteParam, gridColumn.VisibleIndex);

                gridView.UpdateCurrentRow();
            }
        }

        public void ProcessOffline(PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            int[] selectedRowHandles = gridView.GetSelectedRows();
            var gridColumn = gridView.FocusedColumn;
            var copiedValues = copyParser.ToArray();
            var objs = view.SelectedObjects;

            int pasteCount = 0;
            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // select next row in gridview
                var obj = (IXPObject)objs[r];

                // paste cells
                offlinePasteUtils.PasteColumnsToRow(copiedValues[r], obj,
                    listview, pasteParam, gridColumn.VisibleIndex);

                pasteCount++;
            }
            offlinePasteUtils.Logger.Log("{0} rows updated", pasteCount++);
        }
    }
}
