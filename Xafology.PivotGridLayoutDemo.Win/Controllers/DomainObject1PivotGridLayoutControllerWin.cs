using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.XtraPivotGrid;
using Xafology.PivotGrid;
using DevExpress.ExpressApp;
using PivotGridLayoutDemo.Module.BusinessUtils;
using Xafology.ExpressApp.PivotGridLayout.Win.Controllers;

namespace PivotGridLayoutDemo.Win.Controllers
{
    public class DomainObject1PivotGridLayoutControllerWin : PivotGridLayoutControllerWin
    {
        public DomainObject1PivotGridLayoutControllerWin()
        {
            TargetViewId = "DomainObject1_PivotGridView";
            PivotGridSetupObject = new DomainObject1PivotGridSetup();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            PivotGridFieldsMapped += DomainObject1_PivotGridFieldsMapped;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            PivotGridFieldsMapped -= DomainObject1_PivotGridFieldsMapped;
        }

        void DomainObject1_PivotGridFieldsMapped(object sender, Xafology.ExpressApp.PivotGridLayout.PivotGridLayoutEventArgs e)
        {
            PivotGridControl.OptionsDataField.ColumnValueLineCount = 2;
            PivotGridControl.Appearance.Cell.Options.UseTextOptions = true;
            PivotGridControl.Appearance.Cell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            PivotGridControl.Appearance.FieldValue.Options.UseTextOptions = true;
            PivotGridControl.Appearance.FieldValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            PivotGridControl.Appearance.FieldHeader.Options.UseTextOptions = true;
            PivotGridControl.Appearance.FieldHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            PivotGridControl.Appearance.DataHeaderArea.Options.UseTextOptions = true;
            PivotGridControl.Appearance.DataHeaderArea.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            PivotGridControl.Appearance.RowHeaderArea.Options.UseTextOptions = true;
            PivotGridControl.Appearance.RowHeaderArea.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            foreach (PivotGridField field in PivotGridControl.Fields)
            {
                field.Appearance.Header.Options.UseTextOptions = true;
                field.Appearance.Header.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            }
        }
    }
}
