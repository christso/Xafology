using DevExpress.ExpressApp;
using DevExpress.Xpo;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.TestUtils;
using Xafology.ExpressApp.Xpo.Import.Controllers;

namespace Xafology.UnitTests
{
    [TestFixture]
    public class ImportTests  : InMemoryDbTestBase
    {
        [Test]
        public void InsertHeaderCsv()
        {
            var csvStream = GetMockCsvStream();

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();
            
            var map1 = ObjectSpace.CreateObject<HeadersToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeadersToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            param.FieldHeadImportMaps.Add(map1);
            param.FieldHeadImportMaps.Add(map2);

            param.ObjectTypeName = "MockImportObject";
            
            ObjectSpace.CommitChanges(); // why do I need to commit before ObjectTypeInfo is set?

            var importLogic = param.CreateImportLogic(Application, csvStream);
            importLogic.Insert();

            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());

        }

        [Test]
        public void InsertHeaderCsvBeforeCommit()
        {
            var param = GetMockParamObject();
            var csvStream = GetMockCsvStream();
            var importLogic = param.CreateImportLogic(Application, csvStream);
            importLogic.Insert();

            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());
        }

        [Test]
        // Insert csv using controller
        public void InsertHeaderCsvViaConcurrentController()
        {
            var param = GetMockParamObject();
            var csvStream = GetMockCsvStream();
            var controller = new ImportParamDetailViewControllerBase();
            SetupViewController(controller, param);
            param.File.LoadFromStream("data.csv", csvStream);
            controller.Insert();
        }

        [Test]
        public void ImportNoFileToUpload()
        {
            var param = GetMockParamObject();
            var controller = new ImportParamDetailViewControllerBase();
            SetupViewController(controller, param);
            Assert.Throws(typeof(UserFriendlyException), () => controller.AysncImport());
        }

        [Test]
        public void AutoCreateFieldMaps()
        {

        }

        [Test]
        public void ImportOptionsEquals()
        {

        }

        [Test]
        public void UpdateParamObjectTypeNameWillUpdateTypeInfo()
        {

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();
            param.ObjectTypeName = "MockImportObject";
            ObjectSpace.CommitChanges();
            Assert.NotNull(param.ObjectTypeInfo);
        }


        #region Setup
        
        protected override void SetupObjects()
        {
            base.SetupObjects();

        }

        protected override void AddExportedTypes(DevExpress.ExpressApp.ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(ImportHeadersParam));
            module.AdditionalExportedTypes.Add(typeof(MockImportObject));
        }

        #endregion

        #region Utilities

        private void SetupViewController(ViewController controller, IXPObject currentObject)
        {
            controller.Application = Application;
            var view = Application.CreateDetailView(ObjectSpace, currentObject);
            controller.SetView(view);
        }

        private Stream GetMockCsvStream()
        {
            string csvText = @"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30";
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvText);
            return new MemoryStream(csvBytes);
        }

        private ImportHeadersParam GetMockParamObject()
        {
            var map1 = ObjectSpace.CreateObject<HeadersToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeadersToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.FieldHeadImportMaps.Add(map1);
            param.FieldHeadImportMaps.Add(map2);

            param.ObjectTypeName = "MockImportObject";

            return param;
        }

        #endregion

    }
}
