using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.PivotGrid.Win;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.XtraPivotGrid;
using Xafology.ExpressApp.PivotGrid.Controllers;

namespace Xafology.ExpressApp.PivotGrid.Win.Controllers
{
    // Features:
    // does not support default values
    public abstract class PivotGridDrillDownController : PivotGridDrillDownControllerBase
    {

        protected override void OnActivated()
        {
            base.OnActivated();
            View.ControlsCreated += View_ControlsCreated;
        }

        void View_ControlsCreated(object sender, EventArgs e)
        {
            PivotGridListEditor listEditor = ((DevExpress.ExpressApp.ListView)View).Editor as PivotGridListEditor;
            if (listEditor != null)
            {
                listEditor.PivotGridControl.CellDoubleClick += OnPivotGridControlCellDoubleClick;
            }
        }

        protected virtual void OnPivotGridControlCellDoubleClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            ProcessAction(e);
        }

        public void ProcessAction(DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            PivotGridListEditor listEditor = ((DevExpress.ExpressApp.ListView)View).Editor as PivotGridListEditor;

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            PivotDrillDownDataSource drillDown = listEditor.PivotGridControl.CreateDrillDownDataSource(columnIndex, rowIndex);
            List<object> keysToShow = new List<object>();
            for (int i = 0; i < drillDown.RowCount; i++)
            {
                object obj = drillDown[i][0];
                if (obj != null)
                {
                    keysToShow.Add(ObjectSpace.GetKeyValue(obj));
                }
            }

            if (keysToShow.Count > 0)
            {
                string viewId = Application.GetListViewId(View.ObjectTypeInfo.Type);
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(Application.CreateObjectSpace(), View.ObjectTypeInfo.Type, viewId);
                collectionSource.Criteria["SelectedObjects"] = new InOperator(ObjectSpace.GetKeyPropertyName(View.ObjectTypeInfo.Type), keysToShow);
                DevExpress.ExpressApp.ListView listView = Application.CreateListView(viewId, collectionSource, true);
                ShowViewParameters svp = new ShowViewParameters(listView);
                svp.TargetWindow = TargetWindow.NewModalWindow;
                //svp.Context = TemplateContext.View;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
            }
        }
    }
}
