using DevExpress.ExpressApp;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.Actions;
using DevExpress.Data.Filtering;

namespace Xafology.ExpressApp.BatchDelete.Win
{
    public class BatchDeleteListViewControllerWin : BatchDeleteListViewController
    {
        protected override CriteriaOperator ActiveFilterCriteria
        {
            get
            {
                var grid = ((ListView)View).Control as GridControl;
                var filterCriteria = ((GridView)grid.FocusedView).ActiveFilterCriteria;
                return filterCriteria;
            }
        }
        protected override bool ActiveFilterEnabled
        {
            get
            {
                var grid = ((ListView)View).Control as GridControl;
                return ((GridView)grid.FocusedView).ActiveFilterEnabled;
            }
        }
    }
}
