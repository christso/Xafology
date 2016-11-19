using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Paste.Parameters;
using Xafology.ExpressApp.Xpo.ValueMap;

namespace Xafology.ExpressApp.Paste.Win
{
    public class NewRowPasteProcessor
    {
        private readonly ICopyParser copyParser;
        private readonly View view;
        private readonly PasteUtils pasteUtils;
        private readonly OfflinePasteUtils offlinePasteUtils;

        public NewRowPasteProcessor(ICopyParser copyParser, View view)
            : this(copyParser, view, new NullImportLogger())
        {
          
        }

        public NewRowPasteProcessor(ICopyParser copyParser, View view,
            IImportLogger logger)
        {
            if (view == null)
                throw new ArgumentException("Parameter 'view' cannot be null", "view");
            if (copyParser == null)
                throw new ArgumentException("Parameter 'copyParser' cannot be null", "copyParser");

            this.copyParser = copyParser;
            this.view = view;
            this.pasteUtils = new PasteUtils();
            this.offlinePasteUtils = new OfflinePasteUtils(logger);
        }

        public Xpo.ValueMap.IImportLogger Logger
        {
            get { return offlinePasteUtils.Logger; }
        }

        // note that the new row must be focused for this to work
        public void Process(PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();
            var newRowHandle = gridView.FocusedRowHandle;

            if (!gridView.IsNewItemRow(gridView.FocusedRowHandle))
                return;

            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // add new row in gridview
                gridView.FocusedRowHandle = newRowHandle;
                gridView.AddNewRow();

                // paste cells
                pasteUtils.PasteColumnsToRow(copiedValues[r], gridView.FocusedRowHandle,
                    listview, pasteParam);

                gridView.UpdateCurrentRow();
            }
        }

        public void ProcessOffline(PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();
            var newRowHandle = gridView.FocusedRowHandle;
            var os = listview.ObjectSpace;

            if (!gridView.IsNewItemRow(gridView.FocusedRowHandle))
                return;

            int pasteCount = 0;
            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;
                var obj = (IXPObject)os.CreateObject(view.ObjectTypeInfo.Type);
                offlinePasteUtils.PasteColumnsToRow(copiedValues[r], obj,
                    listview, pasteParam);

                pasteCount++;
            }
            offlinePasteUtils.Logger.Log("{0} rows inserted", pasteCount++);
        }

        public void ProcessOffline(PasteParam pasteParam, int commitInterval)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();
            var newRowHandle = gridView.FocusedRowHandle;
            var os = listview.ObjectSpace;

            if (!gridView.IsNewItemRow(gridView.FocusedRowHandle))
                return;

            int pasteCount = 0;
            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {

                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;
                var obj = (IXPObject)os.CreateObject(view.ObjectTypeInfo.Type);
                offlinePasteUtils.PasteColumnsToRow(copiedValues[r], obj,
                    listview, pasteParam);

                // commit batch at each interval
                if (commitInterval > 0 && r > 0 && r % commitInterval == 0)
                    os.CommitChanges();

                pasteCount++;
            }
            os.CommitChanges();
            offlinePasteUtils.Logger.Log("{0} rows inserted", pasteCount++);
        }
    }
}
