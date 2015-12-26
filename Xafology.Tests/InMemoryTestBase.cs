using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.TestUtils
{
    public class InMemoryDbTestBase
    {
        protected string ApplicationName = "Default Application";

        private XPObjectSpaceProvider ObjectSpaceProvider;
        protected XPObjectSpace ObjectSpace;
        protected TestApplication Application;

        [SetUp]
        public void Setup()
        {
            InitializeImageLoader();

            ObjectSpaceProvider = CreateObjectSpaceProvider();

            Application = new TestApplication();

            // add base module
            ModuleBase module = new ModuleBase();
            AddExportedTypes(module);
            Application.Modules.Add(module);

            // set up application
            Application.Setup(ApplicationName, ObjectSpaceProvider);
            Application.CheckCompatibility();
            ObjectSpace = (XPObjectSpace)ObjectSpaceProvider.CreateObjectSpace();

            SetupObjects();
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
            return new XPObjectSpaceProvider(new MemoryDataStoreProvider());
        }

        protected virtual void SetupObjects()
        {
        }

        protected virtual void AddExportedTypes(ModuleBase module)
        {
            // module.AdditionalExportedTypes.Add(typeof(SetOfBooks));
        }

        [TearDown]
        public void TearDown()
        {
            Application = null;
        }
    }
}
