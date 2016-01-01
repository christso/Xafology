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
    public class HeadInsertTests : ImportTestsBase
    {
        [Test]
        public void InsertSimpleHeaderCsv()
        {
            var xpoMapper = new XpoFieldMapper(Application);

            var param = GetHeadMockParamObject();

            string csvText = @"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30";
            var csvStream = GetMockCsvStream(csvText);


            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            loader.Execute();

            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            MockImportObject obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();

            Assert.AreEqual(3, inserted.Count());
            Assert.AreEqual(30, obj.Amount);
            Assert.AreEqual(null, obj.MockLookupObject);
        }

        [Test]
        public void ExceptionIfInsertInvalidHeader()
        {
            var xpoMapper = new XpoFieldMapper(Application);

            string csvText = @"Description,WrongAmount
Hello 1,10
Hello 2,20
Hello 3,30";

            var param = GetHeadMockParamObject();
            var csvStream = GetMockCsvStream(csvText);


            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            
            var ex = Assert.Throws<ArgumentException>(() => loader.Execute());
            Assert.AreEqual("field", ex.ParamName);
        }

        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public void InsertHeaderCsvWithLookup(bool createMember, bool cacheObject)
        {
            #region Arrange Param

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject";
            map3.TargetName = map3.SourceName;
            map3.CreateMember = createMember;
            map3.CacheObject = cacheObject;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);

            param.ObjectTypeName = "MockImportObject";

            #endregion

            #region Arrange Mapper
            string csvText = @"Description,Amount,MockLookupObject
Hello 1,10,Apple
Hello 2,20,Samsung
Hello 3,30,HTC";

            var csvStream = GetMockCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);

            #endregion

            // act

            loader.Execute();

            // assert
            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());
            
            var obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(30, obj.Amount);
            Assert.AreEqual(1, xpoMapper.XpObjectsNotFound.Count());
            Assert.AreEqual(3, xpoMapper.XpObjectsNotFound[typeof(MockLookupObject)].Count());

            // parameterized assert
            if (createMember)
            {
                Assert.NotNull(obj.MockLookupObject);
            }
            else if (!createMember)
            {
                Assert.Null(obj.MockLookupObject);
            }
        }

        [Test]
        public void InsertHeaderCsvWithLookupOfSpecificFields()
        {
            #region Arrange Parameters
            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject";
            map3.TargetName = map3.SourceName;
            map3.CreateMember = true;

            var map4 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map4.SourceName = "MockLookupObject2";
            map4.TargetName = map4.SourceName;
            map4.CreateMember = false;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);
            param.HeaderToFieldMaps.Add(map4);

            param.ObjectTypeName = "MockImportObject";
            #endregion

            #region Arrange Mapper
            string csvText = @"Description,Amount,MockLookupObject,MockLookupObject2
Hello 1,10,Apple,Handset
Hello 2,20,Samsung,Marketing
Hello 3,30,HTC,Credit";

            var csvStream = GetMockCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);

            #endregion

            // act

            loader.Execute();

            // assert
            var inserted = new XPQuery<MockImportObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());

            var obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(30, obj.Amount);
            Assert.NotNull(obj.MockLookupObject);
            Assert.Null(obj.MockLookupObject2);

            Assert.AreEqual(3, xpoMapper.XpObjectsNotFound[typeof(MockLookupObject2)].Count());
            Assert.AreEqual(3, xpoMapper.XpObjectsNotFound[typeof(MockLookupObject)].Count()); // why is this 6?

            Assert.AreEqual(2, xpoMapper.XpObjectsNotFound.Count());
        }

        [Test]
        public void ExceptionIfInsertDuplicateTargets()
        {

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = "Amount";

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();
            param.ObjectTypeName = "MockImportObject";

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);

            string csvText = @"Description,Amount,MockLookupObject
Hello 1,10,Apple
Hello 2,20,Samsung
Hello 3,30,HTC";

            var csvStream = GetMockCsvStream(csvText);

            var request = ObjectSpace.CreateObject<ActionRequest>();
            var logger = new ImportRequestLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);

            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            Assert.Throws<InvalidOperationException>(() => loader.Execute());
        }
    }
}