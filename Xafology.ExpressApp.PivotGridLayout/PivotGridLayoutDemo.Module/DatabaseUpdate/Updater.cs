using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using PivotGridLayoutDemo.Module.BusinessObjects;
using DevExpress.ExpressApp.Security.Strategy;
//using DevExpress.ExpressApp.Reports;
//using DevExpress.ExpressApp.PivotChart;
//using DevExpress.ExpressApp.Security.Strategy;
//using PivotGridLayoutDemo.Module.BusinessObjects;

namespace PivotGridLayoutDemo.Module.DatabaseUpdate
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

            SecuritySystemRole adminRole = ObjectSpace.FindObject<SecuritySystemRole>(
                new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<SecuritySystemRole>();
                adminRole.Name = SecurityStrategy.AdministratorRoleName;
                adminRole.IsAdministrative = true;
                adminRole.Save();
            }
            SecuritySystemUser adminUser = ObjectSpace.FindObject<SecuritySystemUser>(
                new BinaryOperator("UserName", "admin"));
            if (adminUser == null)
            {
                adminUser = ObjectSpace.CreateObject<SecuritySystemUser>();
                adminUser.UserName = "admin";
                adminUser.SetPassword("");
                adminUser.Roles.Add(adminRole);
            }

            string name = "Bob";
            DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            if (theObject == null)
            {
                theObject = ObjectSpace.CreateObject<DomainObject1>();
                theObject.Name = name;
                theObject.Amount = 1000;
                theObject.Category1 = "Payments";
                theObject.Category2 = "Other Payments";
                theObject.TranDate = new DateTime(2014, 5, 6);
            }
            name = "Alice";
            theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            if (theObject == null)
            {
                theObject = ObjectSpace.CreateObject<DomainObject1>();
                theObject.Name = name;
                theObject.Amount = 1000;
                theObject.Category1 = "Receipts";
                theObject.Category2 = "Other Receipts";
                theObject.TranDate = new DateTime(2014, 5, 5);
            }
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
