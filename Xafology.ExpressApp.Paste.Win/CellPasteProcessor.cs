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
    public class CellPasteProcessor
    {
        private readonly ICopyParser copyParser;
        private readonly View view;
        private readonly PasteUtils pasteUtils;

        public CellPasteProcessor(ICopyParser copyParser, View view)
        {
            this.copyParser = copyParser;
            this.view = view;
            this.pasteUtils = new PasteUtils();
        }

        // paste single value into selected rows in the focused column
        public void Process(string copiedValue)
        {
            string[] copiedValues = new string[1];
            copiedValues[0] = copiedValue;
            Process(copiedValues);
        }

        public void Process(string[] copiedValues)
        {
            GridListEditor listEditor = ((ListView)view).Editor as GridListEditor;
            if (listEditor == null) return;

            pasteUtils.PasteColumn(copiedValues, listEditor, view.ObjectSpace);
        }
    }
}
