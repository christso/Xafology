using DevExpress.Xpo;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.TestUtils;

namespace Xafology.UnitTests.Import
{
    [TestFixture]
    public class OrdUpdateTests : Xafology.UnitTests.Import.ImportTestsBase
    {
        public OrdUpdateTests()
        {
            SetTesterDbType(TesterDbType.MsSql);
        }

        [Test]
        public void UpdateSimpleOrdinalCsv()
        {
            // arrange parameters

            var map1 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map1.SourceOrdinal = 0;
            map1.TargetName = "Description";
            map1.IsKeyField = true;

            var map2 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map2.SourceOrdinal = 1;
            map2.TargetName = "Amount";

            var param = ObjectSpace.CreateObject<ImportOrdinalsParam>();

            param.OrdToFieldMaps.Add(map1);
            param.OrdToFieldMaps.Add(map2);

            param.ObjectTypeName = "MockFactObject";

            // arrange XPO objects

            var obj1 = ObjectSpace.CreateObject<MockFactObject>();
            obj1.Description = "Hello 1";
            obj1.Amount = 10;

            var obj2 = ObjectSpace.CreateObject<MockFactObject>();
            obj2.Description = "Hello 2";
            obj2.Amount = 20;

            var obj3 = ObjectSpace.CreateObject<MockFactObject>();
            obj3.Description = "Hello 3";
            obj3.Amount = 30;

            ObjectSpace.CommitChanges();

            // arrange loader

            string csvText = @"Hello 1,100
Hello 2,200
Hello 3,300";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoFieldMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new OrdCsvToXpoUpdater(param, csvStream, xpoFieldMapper, logger);

            // act

            loader.Execute();

            // assert
            var updated = new XPQuery<MockFactObject>(ObjectSpace.Session);

            Assert.AreEqual(3, updated.Count()); // returns 6 because it inserts instead of updates

            MockFactObject result = updated.Where(x => x.Description == "Hello 1").FirstOrDefault();
            Assert.AreEqual(100, result.Amount);

            result = updated.Where(x => x.Description == "Hello 2").FirstOrDefault();
            Assert.AreEqual(200, result.Amount);

            result = updated.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(300, result.Amount);
        }

    }
}
