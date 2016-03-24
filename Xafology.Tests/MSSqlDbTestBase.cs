using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo.DB;
using Xafology.Utils.Data;

namespace Xafology.TestUtils
{
    [TestFixture]
    public class MSSqlDbTestBase : ITest
    {
        public string DataPath = @"D:\CTSO\Data\MSSQL12\Data";
        public string ServerName = @"(localdb)\mssqllocaldb";
        public string DatabaseName = "XafologyUnitTest";
        public bool TearDownFixtureEnabled = false;

        private XPObjectSpaceProvider ObjectSpaceProvider;
        public XPObjectSpace ObjectSpace { get; set; }
        public TestApplication Application { get; set; }

        private readonly ModuleBase module;

        public event EventHandler<EventArgs> SetupEvent;
        public event EventHandler<AddExportedTypesEventArgs> AddExportedTypesEvent;

        public MSSqlDbTestBase()
        {
            module = new ModuleBase();
        }

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            InitializeImageLoader();

            ObjectSpaceProvider = CreateObjectSpaceProvider();

            Application = new TestApplication();

            AddExportedTypes(module);
            Application.Modules.Add(module);

            Application.Setup("", ObjectSpaceProvider);
            Application.CheckCompatibility();
            ObjectSpace = (XPObjectSpace)ObjectSpaceProvider.CreateObjectSpace();
        }

        [SetUp]
        public void Setup()
        {
            if (SetupEvent != null)
                SetupEvent(this, EventArgs.Empty);
        }

        private void InitializeImageLoader()
        {
            var classType = GetType();
            if (!ImageLoader.IsInitialized)
            {
                ImageLoader.Init(new AssemblyResourceImageSource(classType.Assembly.FullName, "Images"));
            }
        }

        private XPObjectSpaceProvider CreateObjectSpaceProvider()
        {
            MSSqlClientHelper.DropDatabase(ServerName, DatabaseName);
            MSSqlClientHelper.CreateDatabase(ServerName, DatabaseName, DataPath);
            string connectionString = MSSqlConnectionProvider.GetConnectionString(ServerName, DatabaseName);
            return new XPObjectSpaceProvider(connectionString, null);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteExportedObjects(module, ObjectSpace.Session);
            ObjectSpace.CommitChanges();
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            if (TearDownFixtureEnabled)
                MSSqlClientHelper.DropDatabase(ServerName, DatabaseName);
        }

        public virtual void AddExportedTypes(ModuleBase module)
        {
            if (AddExportedTypesEvent != null)
                AddExportedTypesEvent(this, new AddExportedTypesEventArgs(module));
        }

        public static void DeleteExportedObjects(ModuleBase module, Session session)
        {
            if (module == null)
                throw new InvalidOperationException("module cannot be null");

            foreach (var type in module.AdditionalExportedTypes)
            {
                DeleteObjects(session, type);
            }
        }

        public static void DeleteObjects(Session session, Type type)
        {
            session.Delete(new XPCollection(session, type));
        }
    }
}
