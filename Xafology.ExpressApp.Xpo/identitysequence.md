Xafology Identity Sequence
=======

Xafology.IdentitySequenceDemo provides a demonstration on how to add a sequential number to your business object. Note that it only supports MS SQL Server.

When you inherit from IdentityBaseObject, an identity column will be added if the column does not already exist (based on the name).

Usage
-------

Inherit from IdentityBaseObject.

```
namespace Xafology.IdentitySequenceDemo.Module.BusinessObjects
{
	public class DomainObject1 : IdentityBaseObject
	{
		// ...
		[PersistentAlias("concat('D', ToStr(SequentialNumber))")]
		public string DomainObjectId
		{
			get
			{
				return Convert.ToString(EvaluateAlias("DomainObjectId"));
			}
		}	
	}
}
```

Add Xafology.ExpressApp.Xpo module to your platform-agnostic module.

```
namespace Xafology.IdentitySequenceDemo.Module
{
    partial class IdentitySequenceDemoModule
    {
        private void InitializeComponent()
        {
            // 
            // IdentitySequenceDemoModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.Xpo.XpoModule));
        }
    }
}
```

Pass the object to SetupIdentityColumn in your module updater.

```
namespace Xafology.IdentitySequenceDemo.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
		// ...
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            Xafology.ExpressApp.Xpo.Updater.SetupIdentityColumn(((XPObjectSpace)ObjectSpace).Session, typeof(DomainObject1));
        }
    }
}
```

Use the object space provider in Xafology.ExpressApp.Xpo in the Win and Web application classes.

```
namespace Xafology.IdentitySequenceDemo.Web
{
    public partial class IdentitySequenceDemoAspNetApplication : WebApplication
    {
		// ...
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new D2NObjectSpaceProvider(args.ConnectionString, args.Connection, true);
        }
	}
}
```

Additional notes
-------

You cannot obtain the SequentialNumber before the object has been saved to the database. You can workaround this problem by executing a SQL query when the object is saved, however, since this will slow down the performance, you may as well use Xafology.SequenceDemo.
