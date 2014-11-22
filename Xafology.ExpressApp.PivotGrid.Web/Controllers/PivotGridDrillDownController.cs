using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.PivotGrid.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;

namespace Xafology.ExpressApp.PivotGrid.Web.Controllers
{
    public abstract class PivotGridDrillDownController : ViewController<ListView>, IXafCallbackHandler
    {
        public PivotGridDrillDownController()
        {
            // Usage: TargetDefaultValuesController = typeof(MyApp.Module.Controllers.MyDefaultValuesViewController);
            TargetDefaultValuesController = null;
        }

        public Type TargetDefaultValuesController { get; set; }

        protected override void OnActivated()
        {
            base.OnActivated();
            WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
        }
        void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            if (View != null)
            {
                ASPxPivotGridListEditor pivotGridListEditor = View.Editor as ASPxPivotGridListEditor;
                if (pivotGridListEditor != null)
                {
                    ASPxPivotGrid pivotGrid = pivotGridListEditor.PivotGridControl;
                    pivotGrid.ClientSideEvents.CellClick = String.Format("function(s, e){{{0}}}", XafCallbackManager.GetScript("ViewController1",
                        "e.ColumnIndex.toString() + ';' + e.RowIndex.toString()"
                        + " + ';' + e.ColumnValueType + ';' + e.RowValueType "));
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            XafCallbackManager.RegisterHandler("ViewController1", this);
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
        }
        protected XafCallbackManager XafCallbackManager
        {
            get
            {
                return ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
            }
        }

        #region IXafCallbackHandler Members

        public void ProcessAction(string parameter)
        {
            string[] indices = parameter.Split(';');
            int columnIndex = Int32.Parse(indices[0]);
            int rowIndex = Int32.Parse(indices[1]);
            string columnValueType = indices[2];
            string rowValueType = indices[3];

            var editor = (ASPxPivotGridListEditor)View.Editor;

            PivotDrillDownDataSource drillDown = editor.PivotGridControl.CreateDrillDownDataSource(columnIndex, rowIndex);
            ArrayList keysToShow = new ArrayList();
            foreach (PivotDrillDownDataRow row in drillDown)
            {
                object key = row[View.ObjectTypeInfo.KeyMember.Name];
                if (key != null)
                {
                    keysToShow.Add(key);
                }
            }

            // Show list view
            string viewId = Application.GetListViewId(View.ObjectTypeInfo.Type);
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(Application.CreateObjectSpace(), View.ObjectTypeInfo.Type, viewId);
            collectionSource.Criteria["SelectedObjects"] = new InOperator(ObjectSpace.GetKeyPropertyName(View.ObjectTypeInfo.Type), keysToShow);
            ListView listView = Application.CreateListView(viewId, collectionSource, true);
            ShowViewParameters svp = new ShowViewParameters(listView);

            svp.TargetWindow = TargetWindow.NewModalWindow;

            // Add Default Values controller
            if (TargetDefaultValuesController != null)
            {
                var pivotFieldValuePairs = GetPivotFieldValues(editor.PivotGridControl, columnIndex, rowIndex);
                Dictionary<string, object> dicPivotFieldValues = GetDefaultValues(pivotFieldValuePairs);
                
                var defaultValuesController = (Xafology.ExpressApp.Controllers.DefaultValuesViewController)Activator.CreateInstance(TargetDefaultValuesController, dicPivotFieldValues);
                svp.Controllers.Add(defaultValuesController);
            }

            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
        }

        #region PivotGrid

        private List<PivotGridFieldValuePair> GetPivotFieldValues(ASPxPivotGrid pivotGridControl, int columnIndex, int rowIndex)
        {
            var pairs = new System.Collections.Generic.List<PivotGridFieldValuePair>();
            foreach (PivotGridField field in pivotGridControl.Fields)
            {
                var fieldValue = new PivotGridFieldValuePair()
                {
                    Field = field,
                    Value = GetPivotFieldValue(pivotGridControl, field, columnIndex, rowIndex)
                };
                pairs.Add(fieldValue);
            }
            return pairs;
        }

        private object GetPivotFieldValue(ASPxPivotGrid pivotGridControl, PivotGridField field, int columnIndex, int rowIndex)
        {
            if (field == null || pivotGridControl == null)
                return null;
            if (field.Area == PivotArea.ColumnArea)
            {
                return pivotGridControl.GetFieldValue(field, columnIndex);
            }
            else if (field.Area == PivotArea.RowArea)
            {
                return pivotGridControl.GetFieldValue(field, rowIndex);
            }
            else if (field.Area == PivotArea.FilterArea)
            {
                if (field.FilterValues.ValuesIncluded.Length == 1)
                    return field.FilterValues.ValuesIncluded[0];
            }
            return null;
        }

        private Dictionary<string, object> GetDefaultValues(List<PivotGridFieldValuePair> pivotFieldValuePairs)
        {
            var dic = new Dictionary<string, object>();
            foreach (PivotGridFieldValuePair pair in pivotFieldValuePairs)
            {
                dic.Add(pair.Field.FieldName, pair.Value);
            }
            return dic;
        }

        private class PivotGridFieldValuePair
        {
            public PivotGridField Field;
            public object Value;
        }

        #endregion

        #endregion
    }
}
