using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
//using DevExpress.ExpressApp.Reports;
//using DevExpress.ExpressApp.PivotChart;
//using DevExpress.ExpressApp.Security.Strategy;
//using Xafology.IdentitySequenceDemo.Module.BusinessObjects;

namespace Xafology.IdentitySequenceDemo.Module.DatabaseUpdate
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
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //Xafology.ExpressApp.Xpo.Updater.SetupIdentityColumn(((XPObjectSpace)ObjectSpace).Session, typeof(DomainObject1));
        }
    }
}
