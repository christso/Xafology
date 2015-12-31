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
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Concurrency;
using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp;

namespace Xafology.UnitTests
{
    [TestFixture]
    public class ImportTests : ImportTestsBase
    {

        [Test]
        public void InsertHeaderCsv()
        {
            var param = GetHeadMockParamObject();
            var csvStream = GetMockCsvStream(@"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30");
            var importLogic = param.CreateCsvToXpoLoader(Application, csvStream);
            importLogic.Insert();

            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());
        }

        [Test]
        // Insert csv using controller
        public void InsertHeaderCsvViaConcurrentController()
        {
            var param = GetHeadMockParamObject();
            var csvStream = GetMockCsvStream(@"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30");
            var controller = new ImportParamDetailViewControllerBase();
            SetupViewController(controller, param);
            param.File.LoadFromStream("data.csv", csvStream);
            controller.Insert();
        }

        [Test]
        public void ImportNoFileToUpload()
        {
            var param = GetHeadMockParamObject();
            var controller = new ImportParamDetailViewControllerBase();
            SetupViewController(controller, param);
            Assert.Throws(typeof(UserFriendlyException), () => controller.AysncImport());
        }

        [Test]
        public void UpdateParamObjectTypeNameWillUpdateTypeInfo()
        {

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();
            param.ObjectTypeName = "MockImportObject";
            ObjectSpace.CommitChanges();
            Assert.NotNull(param.ObjectTypeInfo);
        }

    }
}
