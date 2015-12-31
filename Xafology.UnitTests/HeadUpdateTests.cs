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
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Concurrency;
using Xafology.ExpressApp.Xpo.Import;

namespace Xafology.UnitTests
{
    [TestFixture]
    public class HeadUpdateTests : ImportTestsBase
    {
        [Test]
        public void UpdateSimpleHeaderCsv()
        {
       
            // arrange parameters

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;
            map1.IsKeyField = true;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);

            param.ObjectTypeName = "MockImportObject";

            // arrange XPO objects

            var obj1 = ObjectSpace.CreateObject<MockImportObject>();
            obj1.Description = "Hello 1";
            obj1.Amount = 10;

            var obj2 = ObjectSpace.CreateObject<MockImportObject>();
            obj2.Description = "Hello 2";
            obj2.Amount = 20;

            var obj3 = ObjectSpace.CreateObject<MockImportObject>();
            obj3.Description = "Hello 3";
            obj3.Amount = 30;

            ObjectSpace.CommitChanges();

            // arrange loader

            string csvText = @"Description,Amount
Hello 1,100
Hello 2,200
Hello 3,300";

            var csvStream = GetMockCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoFieldMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new HeadCsvToXpoUpdater(param, csvStream, xpoFieldMapper, logger);

            // act

            loader.Execute();

            // assert
            var updated = new XPQuery<MockImportObject>(ObjectSpace.Session);

            Assert.AreEqual(3, updated.Count()); // returns 6 because it inserts instead of updates

            MockImportObject result = updated.Where(x => x.Description == "Hello 1").FirstOrDefault();
            Assert.AreEqual(100, result.Amount);

            result = updated.Where(x => x.Description == "Hello 2").FirstOrDefault();
            Assert.AreEqual(200, result.Amount);

            result = updated.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(300, result.Amount);
        }

        [Test]
        public void ExceptionIfMultipleKeyFields()
        {

        }
    }
}
