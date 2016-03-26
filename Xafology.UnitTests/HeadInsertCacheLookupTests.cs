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
using Xafology.ImportDemo.Module.BusinessObjects;
using Xafology.ExpressApp.Xpo.ValueMap;

namespace Xafology.ImportDemo.UnitTests
{
    [TestFixture]
    public class HeadInsertCacheLookupTests : ImportTestsBase
    {
        public HeadInsertCacheLookupTests()
        {
            SetTesterDbType(TesterDbType.MsSql);

            var tester = Tester as MSSqlDbTestBase;
            if (tester != null)
                tester.DatabaseName = "XafologyImportTest";
        }

        [Test]
        public void AddToObjectCacheTwice()
        {
            #region Execution 1

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

            param.ObjectTypeName = "MockFactObject";

            string csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Apple,Handset
Hello 2,20,Samsung,Marketing
Hello 3,30,HTC,Credit
Hello 4,30,HTC,Credit";

            var csvStream = ConvertToCsvStream(csvText);
            var request = ObjectSpace.CreateObject<ImportRequest>();
            var logger = new ImportLogger(request);
            var xpoFieldMapper = new XpoFieldMapper();

            HeadCsvToXpoInserter loader = new HeadCsvToXpoInserter(param, csvStream, xpoFieldMapper, logger);
            loader.Execute();

            #endregion

            #region Assert Cache

            var cachedXpObjects = xpoFieldMapper.LookupCacheDictionary;

            Assert.AreEqual(2, cachedXpObjects.Count);

            Assert.AreEqual(3, cachedXpObjects[typeof(MockLookupObject1)].Count);

            var cachedList = cachedXpObjects[typeof(MockLookupObject1)].Cast<MockLookupObject1>();

            Assert.AreEqual(1, cachedList.Where((obj) => (obj).Name == "Apple").Count());
            Assert.AreEqual(1, cachedList.Where((obj) => (obj).Name == "Samsung").Count());
            Assert.AreEqual(1, cachedList.Where((obj) => (obj).Name == "HTC").Count());

            #endregion

            #region Assert Fact

            var factObjs = ObjectSpace.GetObjects<MockFactObject>();
            Assert.AreEqual(4, factObjs.Count);

            var obj1 = factObjs.Where(x => x.Description == "Hello 1").FirstOrDefault();
            Assert.NotNull(obj1.MockLookupObject1);
            Assert.NotNull(obj1.MockLookupObject2);

            var obj2 = factObjs.Where(x => x.Description == "Hello 2").FirstOrDefault();
            Assert.NotNull(obj2.MockLookupObject1);
            Assert.NotNull(obj2.MockLookupObject2);

            var obj3 = factObjs.Where(x => x.Description == "Hello 3").FirstOrDefault();
            Assert.NotNull(obj3.MockLookupObject1);
            Assert.NotNull(obj3.MockLookupObject2);

            var obj4 = factObjs.Where(x => x.Description == "Hello 4").FirstOrDefault();
            Assert.NotNull(obj4.MockLookupObject1);
            Assert.NotNull(obj4.MockLookupObject2);

            #endregion
        }

        [Test]
        public void AddToObjectCacheTwice_Level1()
        {
            #region Arrange

            var xpoFieldMapper = new XpoFieldMapper();

            var typeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject));
            var memberInfo = typeInfo.FindMember("MockLookupObject1");

            #endregion

            #region Act

            var targetObj1 = ObjectSpace.CreateObject<MockFactObject>();
            targetObj1.Description = "Target 1";
            xpoFieldMapper.SetMemberValue(targetObj1, memberInfo, "Apple", true, true);

            var targetObj2 = ObjectSpace.CreateObject<MockFactObject>();
            targetObj2.Description = "Target 2";
            xpoFieldMapper.SetMemberValue(targetObj2, memberInfo, "Apple", true, true);

            ObjectSpace.CommitChanges();

            #endregion

            #region Assert

            var targetObjs = ObjectSpace.GetObjects<MockFactObject>();

            var retargetObj1 = targetObjs.Where(x => x.Description == "Target 1").FirstOrDefault();
            var retargetObj2 = targetObjs.Where(x => x.Description == "Target 2").FirstOrDefault();

            Assert.NotNull(retargetObj1);
            Assert.NotNull(retargetObj1.MockLookupObject1);

            Assert.NotNull(retargetObj2);
            Assert.NotNull(retargetObj2.MockLookupObject1);

            #endregion
        }

        [Test]
        public void AddToObjectCacheTwice_Level2()
        {
            #region Arrange

            var xpoFieldMapper = new XpoFieldMapper();

            var factTypeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject));
            var lookupMemberInfo = factTypeInfo.FindMember("MockLookupObject1");

            var cachedLookupValueConverter = new CachedLookupValueConverter(xpoFieldMapper.LookupCacheDictionary);


            #endregion

            #region Act

            // add key to cache
            xpoFieldMapper.LookupCacheDictionary.Add(new XPCollection(ObjectSpace.Session, lookupMemberInfo.MemberType));

            var targetObj1 = ObjectSpace.CreateObject<MockFactObject>();
            targetObj1.Description = "Target 1";
            var obj1_lookup1 = cachedLookupValueConverter.ConvertToXpObject("Apple", lookupMemberInfo, ObjectSpace.Session, true);
            targetObj1.MockLookupObject1 = (MockLookupObject1)obj1_lookup1;

            var targetObj2 = ObjectSpace.CreateObject<MockFactObject>();
            targetObj2.Description = "Target 2";
            var obj2_lookup1 = cachedLookupValueConverter.ConvertToXpObject("Apple", lookupMemberInfo, ObjectSpace.Session, true); // why is this null?
            targetObj2.MockLookupObject1 = (MockLookupObject1)obj2_lookup1;

            ObjectSpace.CommitChanges();

            #endregion

            #region Assert

            var targetObjs = ObjectSpace.GetObjects<MockFactObject>();

            var retargetObj1 = targetObjs.Where(x => x.Description == "Target 1").FirstOrDefault();
            var retargetObj2 = targetObjs.Where(x => x.Description == "Target 2").FirstOrDefault();

            Assert.NotNull(retargetObj1);
            Assert.NotNull(obj1_lookup1);
            Assert.NotNull(retargetObj1.MockLookupObject1);

            Assert.NotNull(retargetObj2);
            Assert.NotNull(obj2_lookup1);
            Assert.NotNull(retargetObj2.MockLookupObject1);

            #endregion
        }

    }
}