//using DevExpress.ExpressApp.Reports;
using Xafology.ExcelReportDemo.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
//using DevExpress.ExpressApp.PivotChart;
//using DevExpress.ExpressApp.Security.Strategy;
//using Xafology.ExcelReportDemo.Module.BusinessObjects;

namespace Xafology.ExcelReportDemo.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            var currency1 = ObjectSpace.CreateObject<Currency>();
            currency1.Name = "AUD";

            var cashFlow1 = ObjectSpace.CreateObject<CashFlow>();
            cashFlow1.Currency = currency1;
            cashFlow1.Amount = 10000;

            cashFlow1 = ObjectSpace.CreateObject<CashFlow>();
            cashFlow1.Currency = currency1;
            cashFlow1.Amount = 20000;

        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
