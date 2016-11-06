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
using Xafology.ExpressApp.Xpo.Import.Logic;

namespace Xafology.ExpressApp.Paste.Win
{
    public class PasteViewController : ViewController
    {
        private readonly DevExpress.Utils.DefaultBoolean copyColumnHeaders;

        const string pasteRowsCaption = "Paste Rows";
        const string pasteNewRowsCaption = "Paste Rows Offline";
        const string pasteColumnCaption = "Paste Column";
        const string clearColumnCaption = "Clear Column";

        private SimpleImportLogger logger;

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

            var pasteRowsOfflineChoice = new ChoiceActionItem();
            pasteRowsOfflineChoice.Caption = pasteNewRowsCaption;
            pasteAction.Items.Add(pasteRowsOfflineChoice);

            var pasteColumnChoice = new ChoiceActionItem();
            pasteColumnChoice.Caption = pasteColumnCaption;
            pasteAction.Items.Add(pasteColumnChoice);

            var clearColumnChoice = new ChoiceActionItem();
            clearColumnChoice.Caption = clearColumnCaption;
            pasteAction.Items.Add(clearColumnChoice);

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
            logger = new SimpleImportLogger();

            // find param
            var pasteParam = ObjectSpace.FindObject<PasteParam>(CriteriaOperator.Parse("IsDefault=true"));

            switch (e.SelectedChoiceActionItem.Caption)
            {
                case pasteRowsCaption:
                    PasteRowValues(pasteParam);
                    break;
                case pasteNewRowsCaption:
                    PasteOfflineRowValues(pasteParam, true);
                    break;
                case pasteColumnCaption:
                    PasteColumnValues();
                    break;
                case clearColumnCaption:
                    ClearColumnValues();
                    break;
            }
        }

        private void PasteOfflineRowValues(PasteParam pasteParam, bool isRoot = false)
        {
            // create Paste Processor
            var clipboard = new Clipboard();
            var clipboardParser = new CopyParser(clipboard);
            var newRowPasteProcessor = new NewRowPasteProcessor(clipboardParser, this.View, logger);
            var existingRowPasteProcessor = new ExistingRowPasteProcessor(clipboardParser, this.View, logger);

            string[][] copiedValues = clipboardParser.ToArray();
            if (copiedValues == null) return;

            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;

            if (listEditor != null)
            {
                var gridView = listEditor.GridView;

                if ((gridView.IsNewItemRow(gridView.FocusedRowHandle)))
                {
                    // paste to new rows
                    newRowPasteProcessor.ProcessOffline(pasteParam);
                }
                else
                {
                    existingRowPasteProcessor.ProcessOffline(pasteParam);
                }
                logger.Log("Please SAVE or CANCEL your changes.");
                if (isRoot)
                {
                    new Xafology.ExpressApp.SystemModule.GenericMessageBox(
                        logger.LogMessage, "Import SUCCESSFUL");
                }
            }
            
        }

        private void PasteRowValues(PasteParam pasteParam)
        {
            // create clipboard parser
            var clipboard = new Clipboard();
            var clipboardParser = new CopyParser(clipboard);

            // validate

            string[][] copiedValues = clipboardParser.ToArray();
            if (copiedValues == null) return;

            if (copiedValues.GetLength(0) > PasteSettings.MaximumOnlineRows)
            {
                logger.Log("Rows exceeded the maximum of {0}. OFFLINE mode used.",
                    PasteSettings.MaximumOnlineRows);
                PasteOfflineRowValues(pasteParam, false);
                new Xafology.ExpressApp.SystemModule.GenericMessageBox(
                    logger.LogMessage, "Import SUCCESSFUL");
                return;
            }
            // create Paste Processor
            var newRowPasteProcessor = new NewRowPasteProcessor(clipboardParser, this.View);
            var existingRowPasteProcessor = new ExistingRowPasteProcessor(clipboardParser, this.View);

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
                    existingRowPasteProcessor.Process(pasteParam);
                }
            }
        }

        private void PasteColumnValues()
        {
            var clipboard = new Clipboard();
            var clipboardParser = new CopyParser(clipboard);
            var cellPasteProcessor = new CellPasteProcessor(clipboardParser, this.View);

            string[] copiedValues = clipboardParser.ToArray(0);
            if (copiedValues == null) return;
            cellPasteProcessor.Process(copiedValues);
        }

        private void ClearColumnValues()
        {
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor == null) return;
            var gridView = listEditor.GridView;
            var gridColumn = gridView.FocusedColumn;
            var columnKey = gridColumn.FieldName.Replace("!", string.Empty);
            var member = listEditor.Model.ModelClass.OwnMembers[columnKey]; // TODO: remove ! suffice when column is a lookup

            if (!member.AllowEdit)
                throw new InvalidOperationException("Column '" + gridView.FocusedColumn.Caption + "' is not editable.");

            int[] selectedRows = gridView.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {
                // paste object to focused cell
                gridView.SetRowCellValue(rowHandle, gridView.FocusedColumn, null);
            }
        }
    }
}
