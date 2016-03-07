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
        public ITest tester;
        private readonly ITest memTestBase;
        private readonly ITest mssqlTestBase;

        public XPObjectSpace ObjectSpace
        {
            get
            {
                return tester.ObjectSpace;
            }

            set
            {
                tester.ObjectSpace = value;
            }
        }

        public TestApplication Application
        {
            get
            {
                return tester.Application;
            }
            set
            {
                tester.Application = value;
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

            this.tester = memTestBase; // default is inmemory
        }

        public void SetTesterDbType(TesterDbType testerDbType)
        {
            switch (testerDbType)
            {
                case TesterDbType.InMemory:
                    tester = memTestBase;
                    break;
                case TesterDbType.MsSql:
                    tester = mssqlTestBase;
                    break;
            }
        }

        [SetUp]
        public void Setup()
        {
            tester.Setup();
        }

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            tester.SetUpFixture();
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
            tester.TearDown();
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            tester.TearDownFixture();
        }
    }
}
