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

using Xafology.ExpressApp.Xpo.Import;


namespace Xafology.UnitTests.Import
{
    [TestFixture]
    public class OrdInsertTests : Xafology.UnitTests.Import.ImportTestsBase
    {
        [Test]
        public void InsertSimpleOrdCsv()
        {
            // arrange parameters

            var map1 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map1.SourceOrdinal = 0;
            map1.TargetName = "Description";

            var map2 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map2.SourceOrdinal = 1;
            map2.TargetName = "Amount";

            var param = ObjectSpace.CreateObject<ImportOrdinalsParam>();

            param.OrdToFieldMaps.Add(map1);
            param.OrdToFieldMaps.Add(map2);

            param.ObjectTypeName = "MockFactObject";

            string csvText = @"Hello 1,10
Hello 2,20
Hello 3,30";
            var csvStream = ConvertToCsvStream(csvText);

            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new OrdCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            loader.Execute();

            var inserted = new XPQuery<MockFactObject>(ObjectSpace.Session);
            MockFactObject obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();

            Assert.AreEqual(3, inserted.Count());
            Assert.AreEqual(30, obj.Amount);
            Assert.AreEqual(null, obj.MockLookupObject1);
        }

        [Test]
        public void ThrowExceptionIfObjectTypeInfoIsNull()
        {
            var map1 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map1.SourceOrdinal = 0;
            map1.TargetName = "Description";

            var map2 = ObjectSpace.CreateObject<OrdinalToFieldMap>();
            map1.SourceOrdinal = 1;
            map1.TargetName = "Amount";

            var param = ObjectSpace.CreateObject<ImportOrdinalsParam>();

            param.OrdToFieldMaps.Add(map1);
            param.OrdToFieldMaps.Add(map2);

            string csvText = @"Hello 1,10
Hello 2,20
Hello 3,30";
            var csvStream = ConvertToCsvStream(csvText);

            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);

            var ex = Assert.Throws<ArgumentException>(() =>
                new OrdCsvToXpoInserter(param, csvStream, xpoMapper, logger));
            Assert.AreEqual("ObjectTypeInfo", ex.ParamName);
        }
    }
}
