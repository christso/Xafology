using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Linq;
using Xafology.ExpressApp.Xpo.DbIdentity;

namespace Xafology.ExpressApp.Xpo.DatabaseUpdate
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            var subTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(IdentityBaseObject)));

            foreach (Type subType in subTypes)
            {
                Xafology.ExpressApp.Xpo.Updater.SetupIdentityColumn(((XPObjectSpace)ObjectSpace).Session, subType);
            }
        }
    }
}
