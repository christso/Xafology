using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.PivotGridLayout.Web.Controllers;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;

namespace PivotGridLayoutDemo2.Module.Web.Controllers
{
    public class DomainObject1PivotGridLayoutControllerWeb : PivotGridLayoutControllerWeb
    {
        public DomainObject1PivotGridLayoutControllerWeb()
        {
            TargetViewId = "DomainObject1_PivotGridView";
            //PivotGridSetupObject = new DomainObject1PivotGridSetup();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            PivotGridFieldsMapped += DomainObject1_PivotGridFieldsMapped;
        }

        private void DomainObject1_PivotGridFieldsMapped(object sender, Xafology.ExpressApp.PivotGridLayout.PivotGridLayoutEventArgs e)
        {
            foreach (PivotGridField field in PivotGridControl.Fields)
            {
                field.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                field.ValueStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                field.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            PivotGridFieldsMapped -= DomainObject1_PivotGridFieldsMapped;
        }
     
    }
}
