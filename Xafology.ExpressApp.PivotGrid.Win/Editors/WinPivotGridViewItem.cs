using System;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraPivotGrid;
using System.Diagnostics;
using Xafology.ExpressApp.Editors;
using Xafology.ExpressApp.PivotGrid.Editors;

namespace Xafology.ExpressApp.PivotGrid.Win.Editors
{
    public interface IModelWinPivotGridViewItem
    {
        [Category("Data")]
        string CustomControlTypeName { get; set; }
    }
    [ViewItem(typeof(IModelPivotGridViewItem))]
    public class WinPivotGridViewItem : CustomUserControlViewItem
    {
        protected IModelWinPivotGridViewItem model;
        protected IMasterUserControl masterUserControl;

        protected override object CreateControlCore()
        {
            //You can access the View and other properties here to additionally initialize your control.
            return DevExpress.Persistent.Base.ReflectionHelper.CreateObject(model.CustomControlTypeName);
        }
        private PivotGridControl pivotGridControl1;
        public WinPivotGridViewItem(IModelViewItem model, Type objectType)
            : base(model, objectType)
        {
            this.model = model as IModelWinPivotGridViewItem;
            if (this.model == null)
                throw new ArgumentNullException("IModelWinCustomUserControlViewItem must extend IModelCustomUserControlViewItem in the ExtendModelInterfaces method of your Win ModuleBase descendant.");
        }

        protected override void OnControlCreated()
        {
            base.OnControlCreated();
            masterUserControl = Control as IMasterUserControl;

            if (masterUserControl.UserControls.Count > 0)
                pivotGridControl1 = masterUserControl.UserControls[0] as PivotGridControl;
            pivotGridControl1.CellDoubleClick += pivotGridControl_CellDoubleClick;
        }

        void pivotGridControl_CellDoubleClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            // TODO: move to Win Forms
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            // add custom columns
            var columns = new List<string>();
            foreach (PivotGridFieldBase field in pivotGridControl1.Fields)
            {
                columns.Add(field.FieldName);
            }

            // print selected fields

            foreach (PivotGridField field in pivotGridControl1.Fields)
            {
                // print Field Name and Value
                // TODO: ignore fields if there is a field in a lower hierarchy
                var pivotValue = e.GetFieldValue(field);
                if (pivotValue != null)
                {
                    Debug.Print(string.Format("{0} = {1}", field.FieldName, pivotValue));

                }
            }
        }
    }
}
