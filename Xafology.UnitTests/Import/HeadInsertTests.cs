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
using System.Collections;
using System.Diagnostics;

namespace Xafology.UnitTests.Import
{
    [TestFixture]
    public class HeadInsertTests : Xafology.UnitTests.Import.ImportTestsBase
    {
        public HeadInsertTests()
        {
            SetTesterDbType(TesterDbType.MsSql);

            var tester = Tester as MSSqlDbTestBase;
            if (tester != null)
                tester.DatabaseName = "XafologyImportTest";
        }

        [Test]
        public void InsertFull()
        {
            #region Arrange

            var csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Parent 1,Parent B1
Hello 2,11,Parent 2,Parent B2
Hello 3,12,Parent 3,Parent B3
Hello 4,13,Parent 4,Parent B4
";

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject1";
            map3.CreateMember = true;
            map3.TargetName = map3.SourceName;

            var map4 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map4.SourceName = "MockLookupObject2";
            map4.CreateMember = true;
            map4.TargetName = map4.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);
            param.HeaderToFieldMaps.Add(map4);

            param.ObjectTypeName = "MockFactObject";

            ObjectSpace.CommitChanges();

            #endregion

            #region Act

            var csvStream = ConvertToCsvStream(csvText);
            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, null);
            loader.Execute();
            ObjectSpace.CommitChanges();

            #endregion

            #region Assert

            var inserted = new XPQuery<MockFactObject>(ObjectSpace.Session);
            Assert.AreEqual(4, inserted.Count());
            var obj1 = inserted.Where(x => x.Description == "Hello 1").FirstOrDefault();
            var obj2 = inserted.Where(x => x.Description == "Hello 2").FirstOrDefault();
            var obj3 = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();
            var obj4 = inserted.Where(x => x.Description == "Hello 4").FirstOrDefault();

            Assert.NotNull(obj1.MockLookupObject1);
            Assert.NotNull(obj1.MockLookupObject2);
            Assert.NotNull(obj2.MockLookupObject1);
            Assert.NotNull(obj2.MockLookupObject2);
            Assert.NotNull(obj3.MockLookupObject1);
            Assert.NotNull(obj3.MockLookupObject2);
            Assert.NotNull(obj4.MockLookupObject1);
            Assert.NotNull(obj4.MockLookupObject2);

            #endregion
        }

        [Test]
        public void InsertLog()
        {
            // arrange
            var csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Parent 1,Parent B1
Hello 2,11,Parent 2,Parent B2
Hello 3,12,Parent 3,Parent B3
Hello 4,13,Parent 4,Parent B4
";

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject1";
            map3.TargetName = map3.SourceName;

            var map4 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map4.SourceName = "MockLookupObject2";
            map4.TargetName = map4.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);
            param.HeaderToFieldMaps.Add(map4);

            param.ObjectTypeName = "MockFactObject";


            // act

            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);

            var csvStream = ConvertToCsvStream(csvText);
            var xpoMapper = new XpoFieldMapper(Application, logger);

            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            loader.Execute();

            // assert

            Assert.AreEqual(@"Lookup type 'MockLookupObject1' with value 'Parent 1 not found.
Lookup type 'MockLookupObject2' with value 'Parent B1 not found.
Lookup type 'MockLookupObject1' with value 'Parent 2 not found.
Lookup type 'MockLookupObject2' with value 'Parent B2 not found.
Lookup type 'MockLookupObject1' with value 'Parent 3 not found.
Lookup type 'MockLookupObject2' with value 'Parent B3 not found.
Lookup type 'MockLookupObject1' with value 'Parent 4 not found.
Lookup type 'MockLookupObject2' with value 'Parent B4 not found.
4 records inserted.",
                request.RequestLog);
        }

        [Test]
        public void InsertSimpleHeaderCsv()
        {
            var xpoMapper = new XpoFieldMapper(Application);

            var param = GetHeadMockParamObject();

            string csvText = @"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30";
            var csvStream = ConvertToCsvStream(csvText);

            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, null);
            loader.Execute();

            var inserted = new XPQuery<MockFactObject>(ObjectSpace.Session);
            MockFactObject obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();

            Assert.AreEqual(3, inserted.Count());
            Assert.AreEqual(30, obj.Amount);
            Assert.AreEqual(null, obj.MockLookupObject1);
        }

        [Test]
        public void InsertFromStream()
        {
            // arrange

            var param = GetHeadMockParamObject();
            string inpText = @"Description,Amount
Hello 1,10
Hello 2,20
Hello 3,30";

            var inpStream = ConvertToCsvStream(inpText);
            param.File.LoadFromStream("File", inpStream);

            // act
            var outStream = new MemoryStream();
            param.File.SaveToStream(outStream);

            StreamReader reader = new StreamReader(outStream);
            outStream.Position = 0;
            var outText = reader.ReadToEnd();

            // assert
            Assert.AreEqual(inpText, outText);

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
            var csvStream = ConvertToCsvStream(csvText);


            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            
            var ex = Assert.Throws<ArgumentException>(() => loader.Execute());
            Assert.AreEqual("field", ex.ParamName);
        }

        //[TestCase(true, false)]
        //[TestCase(false, false)]
        //[TestCase(true, true)]
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
            map3.SourceName = "MockLookupObject1";
            map3.TargetName = map3.SourceName;
            map3.CreateMember = createMember;
            map3.CacheObject = cacheObject;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);

            param.ObjectTypeName = "MockFactObject";

            #endregion

            #region Arrange Mapper
            string csvText = @"Description,Amount,MockLookupObject1
Hello 1,10,Apple
Hello 2,20,Samsung
Hello 3,30,HTC";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoFieldMapper = new XpoFieldMapper(Application);
            HeadCsvToXpoInserter loader = new HeadCsvToXpoInserter(param, csvStream, xpoFieldMapper, logger);

            #endregion

            // act
            xpoFieldMapper.LookupsNotFound.Add(typeof(MockLookupObject1), new List<string>() { "Apple", "Samsung", "HTC" });

            loader.Execute();

            // assert
            var inserted = new XPQuery<MockFactObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());
            
            var obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(30, obj.Amount);

            Assert.AreEqual(3, xpoFieldMapper.LookupsNotFound[typeof(MockLookupObject1)].Count());

            // parameterized assert
            if (createMember)
            {
                Assert.NotNull(obj.MockLookupObject1);
            }
            else if (!createMember)
            {
                Assert.Null(obj.MockLookupObject1);
            }
        }

        [Test]
        public void AddLookupObject()
        {

            var xpoFieldMapper = new XpoFieldMapper(Application);
            var targetObj = ObjectSpace.CreateObject<MockFactObject>();
            ObjectSpace.CommitChanges();

            var typeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject));
            var memberInfo = typeInfo.FindMember("MockLookupObject1");

            xpoFieldMapper.SetMemberValue(targetObj, memberInfo, "Apple", true, true);

            Assert.AreEqual(1, xpoFieldMapper.LookupsNotFound.Count());
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
            map3.SourceName = "MockLookupObject1";
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

            param.ObjectTypeName = "MockFactObject";
            #endregion

            #region Arrange Mapper
            string csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Apple,Handset
Hello 2,20,Samsung,Marketing
Hello 3,30,HTC,Credit";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);

            #endregion

            // act

            loader.Execute();

            // assert
            var inserted = new XPQuery<MockFactObject>(ObjectSpace.Session);
            Assert.AreEqual(3, inserted.Count());

            var obj = inserted.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.AreEqual(30, obj.Amount);
            Assert.NotNull(obj.MockLookupObject1);
            Assert.Null(obj.MockLookupObject2);

            Assert.AreEqual(3, xpoMapper.LookupsNotFound[typeof(MockLookupObject2)].Count());
            Assert.AreEqual(3, xpoMapper.LookupsNotFound[typeof(MockLookupObject1)].Count()); // why is this 6?

            Assert.AreEqual(2, xpoMapper.LookupsNotFound.Count());
        }

        [Test]
        public void AddToObjectCache()
        {
            #region Arrange

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject1";
            map3.TargetName = map3.SourceName;
            map3.CreateMember = true;
            map3.CacheObject = true;

            var map4 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map4.SourceName = "MockLookupObject2";
            map4.TargetName = map4.SourceName;
            map4.CreateMember = true;
            map4.CacheObject = true;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);
            param.HeaderToFieldMaps.Add(map4);

            #endregion

            #region Act

            param.ObjectTypeName = "MockFactObject";

            string csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Apple,Handset
Hello 2,20,Samsung,Marketing
Hello 3,30,HTC,Credit";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoFieldMapper = new XpoFieldMapper(Application);

            var loader = new HeadCsvToXpoInserter(param, csvStream, xpoFieldMapper, logger);
            loader.Execute();

            #endregion

            #region Assert Cached Objects

            var cachedXpObjects = xpoFieldMapper.LookupCacheDictionary;

            Assert.AreEqual(2, cachedXpObjects.Count);

            Assert.AreEqual(3, cachedXpObjects[typeof(MockLookupObject1)].Count);

            var cachedList = cachedXpObjects[typeof(MockLookupObject1)].Cast<MockLookupObject1>();

            Assert.NotNull(cachedList
                .Where((obj) => (obj).Name == "Apple").FirstOrDefault());
            Assert.NotNull(cachedList
                .Where((obj) => (obj).Name == "Samsung").FirstOrDefault());
            Assert.NotNull(cachedList
                .Where((obj) => (obj).Name == "HTC").FirstOrDefault());

            #endregion

            #region Assert Imported Data

            var factObjs = ObjectSpace.GetObjects<MockFactObject>();
            string output = "";
            foreach (var obj in factObjs)
                output += string.Format("{0},{1},{2}",
                    obj.Description,
                    obj.Amount,
                    obj.MockLookupObject1 == null ? "NULL" : obj.MockLookupObject1.Name,
                    obj.MockLookupObject2 == null ? "NULL" : obj.MockLookupObject2.Name)
                    + "\n";
            #endregion
            Debug.Print(output);

            Assert.AreEqual(3, factObjs.Count);
        }

        [Test]
        public void AddToObjectCacheTwice()
        {
            #region Arrange

            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject1";
            map3.TargetName = map3.SourceName;
            map3.CreateMember = true;
            map3.CacheObject = true;

            var map4 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map4.SourceName = "MockLookupObject2";
            map4.TargetName = map4.SourceName;
            map4.CreateMember = true;
            map4.CacheObject = true;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);
            param.HeaderToFieldMaps.Add(map4);

            #endregion

            #region Act

            param.ObjectTypeName = "MockFactObject";

            string csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Apple,Handset
Hello 2,20,Samsung,Marketing
Hello 3,30,HTC,Credit";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoFieldMapper = new XpoFieldMapper(Application);

            var loader = new HeadCsvToXpoInserter(param, csvStream, xpoFieldMapper, logger);
            loader.Execute();

            csvStream.Seek(0, SeekOrigin.Begin);
            var loader2 = new HeadCsvToXpoInserter(param, csvStream, xpoFieldMapper, logger);
            loader2.Execute();

            #endregion

            #region Assert

            var factObjs = ObjectSpace.GetObjects<MockFactObject>();
            Assert.AreEqual(6, factObjs.Count);

            var cachedXpObjects = xpoFieldMapper.LookupCacheDictionary;

            Assert.AreEqual(2, cachedXpObjects.Count);

            Assert.AreEqual(3, cachedXpObjects[typeof(MockLookupObject1)].Count);

            var cachedList = cachedXpObjects[typeof(MockLookupObject1)].Cast<MockLookupObject1>();

            Assert.AreEqual(2, cachedList.Where((obj) => (obj).Name == "Apple").Count());
            Assert.AreEqual(2, cachedList.Where((obj) => (obj).Name == "Samsung").Count());
            Assert.AreEqual(2, cachedList.Where((obj) => (obj).Name == "HTC").Count());

            #endregion

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
            param.ObjectTypeName = "MockFactObject";

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);

            string csvText = @"Description,Amount,MockLookupObject1
Hello 1,10,Apple
Hello 2,20,Samsung
Hello 3,30,HTC";

            var csvStream = ConvertToCsvStream(csvText);

            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoMapper = new XpoFieldMapper(Application);

            ICsvToXpoLoader loader = new HeadCsvToXpoInserter(param, csvStream, xpoMapper, logger);
            Assert.Throws<InvalidOperationException>(() => loader.Execute());
        }
    }
}