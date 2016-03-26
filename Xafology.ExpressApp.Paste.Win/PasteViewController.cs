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

namespace Xafology.ExpressApp.Paste.Win
{
    public class PasteViewController : ViewController
    {
        private readonly DevExpress.Utils.DefaultBoolean copyColumnHeaders;

        private CopyParser clipboardParser;
        private Clipboard clipboard;
        private NewRowPasteProcessor newRowPasteProcessor;
        private ExistingRowPasteProcessor existingRowPasteProcessor;

        const string executeCaption = "Execute";
        const string optionsCaption = "Options";

        public PasteViewController()
        {

            // if set to true, then you need to remove the column headers before pasting it to another row
            this.copyColumnHeaders = DevExpress.Utils.DefaultBoolean.True;
            TargetViewType = ViewType.ListView;

            var pasteAction = new SingleChoiceAction(this, "PasteAction", PredefinedCategory.ObjectsCreation);
            pasteAction.Caption = "Paste";
            pasteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            pasteAction.Execute += PasteRowAction_Execute;
            var executeChoice = new ChoiceActionItem();
            executeChoice.Caption = executeCaption;
            pasteAction.Items.Add(executeChoice);

            var optionsChoice = new ChoiceActionItem();
            optionsChoice.Caption = optionsCaption;
            pasteAction.Items.Add(optionsChoice);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // whether copying will include column headers
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                listEditor.GridView.OptionsClipboard.CopyColumnHeaders = copyColumnHeaders;

                // create NewRowPasteProcessor
                clipboard = new Clipboard();
                clipboardParser = new CopyParser(clipboard);
                newRowPasteProcessor = new NewRowPasteProcessor(clipboardParser, this.View);
                existingRowPasteProcessor = new ExistingRowPasteProcessor(clipboardParser, this.View);
            }
        }

        private void PasteRowAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case executeCaption:
                    string[][] copiedValues = clipboardParser.ToArray();
                    if (copiedValues == null) return;
                    PasteRowValues(copiedValues);
                    break;

                case optionsCaption:
                    // to be implemented
                    break;
            }

        }

        private void PasteRowValues(string[][] copiedValues)
        {
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;

            if (listEditor != null)
            {
                var gridView = listEditor.GridView;
                Session session = ((XPObjectSpace)ObjectSpace).Session;

                if ((gridView.IsNewItemRow(gridView.FocusedRowHandle)))
                {
                    // paste to new rows
                    newRowPasteProcessor.Process();
                }
                else
                {
                    // paste to selected rows
                    int[] selectedRowHandles = gridView.GetSelectedRows();
                    existingRowPasteProcessor.Process(selectedRowHandles);
                }
            }
        }
    }
}
