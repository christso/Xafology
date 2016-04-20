using DevExpress.ExpressApp;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;

namespace Xafology.ExpressApp.BatchDelete.Web
{
    public class BatchDeleteListViewControllerWeb : BatchDeleteListViewController
    {
        protected override CriteriaOperator ActiveFilterCriteria
        {
            get
            {
                ListEditor editor = ((ListView)View).Editor;
                ASPxGridView grid = ((ASPxGridView)editor.Control);
                var filterCriteria = CriteriaOperator.Parse(grid.FilterExpression);
                return filterCriteria;
            }
        }
        protected override bool ActiveFilterEnabled
        {
            get
            {
                ListEditor editor = ((ListView)View).Editor;
                var grid = ((ASPxGridView)editor.Control);
                return grid.FilterEnabled;
            }
        }
    }
}
