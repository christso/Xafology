using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Persistent.Base;
using Xafology.ExpressApp.Paste.Parameters;

namespace Xafology.ExpressApp.Paste.Win
{
    public class PasteViewController : ViewController
    {
        private readonly DevExpress.Utils.DefaultBoolean copyColumnHeaders;

        const string pasteRowsCaption = "Paste Rows";
        const string pasteCellsCaption = "Paste Cells";

        public PasteViewController()
        {
            // if set to true, then you need to remove the column headers before pasting it to another row
            this.copyColumnHeaders = DevExpress.Utils.DefaultBoolean.True;
            TargetViewType = ViewType.ListView;

            var pasteAction = new SingleChoiceAction(this, "PasteAction", PredefinedCategory.ObjectsCreation);
            pasteAction.Caption = "Paste";
            pasteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            pasteAction.ShowItemsOnClick = true;
            pasteAction.Execute += PasteRowAction_Execute;

            var pasteRowChoice = new ChoiceActionItem();
            pasteRowChoice.Caption = pasteRowsCaption;
            pasteAction.Items.Add(pasteRowChoice);

            var pasteCellsChoice = new ChoiceActionItem();
            pasteCellsChoice.Caption = pasteCellsCaption;
            pasteAction.Items.Add(pasteCellsChoice);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var listView = (ListView)View;
            if (listView.AllowEdit)
                this.Active["AllowEdit"] = true;
            else
                this.Active.SetItemValue("AllowEdit", false);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // whether copying will include column headers
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                listEditor.GridView.OptionsClipboard.CopyColumnHeaders = copyColumnHeaders;

            }
        }

        private void PasteRowAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            // find param
            var pasteParam = ObjectSpace.FindObject<PasteParam>(CriteriaOperator.Parse("IsDefault=true"));

            switch (e.SelectedChoiceActionItem.Caption)
            {
                case pasteRowsCaption:
                    PasteRowValues(pasteParam);
                    break;
                case pasteCellsCaption:
                    PasteCellValues();
                    break;
            }
        }

        private void PasteRowValues(PasteParam pasteParam)
        {
            // create Paste Processor
            var clipboard = new Clipboard();
            var clipboardParser = new CopyParser(clipboard);
            var newRowPasteProcessor = new NewRowPasteProcessor(clipboardParser, this.View);
            var existingRowPasteProcessor = new ExistingRowPasteProcessor(clipboardParser, this.View);
        

            string[][] copiedValues = clipboardParser.ToArray();
            if (copiedValues == null) return;

            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;

            if (listEditor != null)
            {
                var gridView = listEditor.GridView;

                if ((gridView.IsNewItemRow(gridView.FocusedRowHandle)))
                {
                    // paste to new rows
                    newRowPasteProcessor.Process(pasteParam);
                }
                else
                {
                    // paste to selected rows
                    int[] selectedRowHandles = gridView.GetSelectedRows();
                    existingRowPasteProcessor.Process(selectedRowHandles, pasteParam);
                }
            }
        }

        private void PasteCellValues()
        {
            var clipboard = new Clipboard();
            var clipboardParser = new CopyParser(clipboard);
            var cellPasteProcessor = new CellPasteProcessor(clipboardParser, this.View);

            string[] copiedValues = clipboardParser.ToArray(0);
            if (copiedValues == null) return;
            cellPasteProcessor.Process(copiedValues);
        }

    }
}
