using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp;


namespace Xafology.TestUtils
{
    public class TestBase
    {

        private readonly ITest memTestBase;
        private readonly ITest mssqlTestBase;

        public ITest Tester
        {
            get; set;
        }

        public XPObjectSpace ObjectSpace
        {
            get
            {
                return Tester.ObjectSpace;
            }

            set
            {
                Tester.ObjectSpace = value;
            }
        }

        public TestApplication Application
        {
            get
            {
                return Tester.Application;
            }
            set
            {
                Tester.Application = value;
            }
        }

        public TestBase()
        {
            memTestBase = new InMemoryDbTestBase();
            memTestBase.SetupEvent += Tester_OnSetup;
            memTestBase.AddExportedTypesEvent += Tester_OnAddExportedTypes;

            mssqlTestBase = new MSSqlDbTestBase();
            mssqlTestBase.SetupEvent += Tester_OnSetup;
            mssqlTestBase.AddExportedTypesEvent += Tester_OnAddExportedTypes;

            this.Tester = memTestBase; // default is inmemory
        }

        public void SetTesterDbType(TesterDbType testerDbType)
        {
            switch (testerDbType)
            {
                case TesterDbType.InMemory:
                    Tester = memTestBase;
                    break;
                case TesterDbType.MsSql:
                    Tester = mssqlTestBase;
                    break;
            }
        }

        [SetUp]
        public void Setup()
        {
            Tester.Setup();
        }

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            Tester.SetUpFixture();
        }

        private void Tester_OnSetup(object sender, EventArgs e)
        {
            OnSetup();
        }

        private void Tester_OnAddExportedTypes(object sender, AddExportedTypesEventArgs e)
        {
            OnAddExportedTypes(e.Module);
        }

        public virtual void OnAddExportedTypes(ModuleBase module)
        {

        }

        public virtual void OnSetup()
        {

        }

        [TearDown]
        public void TearDown()
        {
            Tester.TearDown();
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            Tester.TearDownFixture();
        }
    }
}
