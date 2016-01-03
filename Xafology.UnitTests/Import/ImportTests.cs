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
using Xafology.ExpressApp;
using System.Diagnostics;
using Xafology.Utils;

namespace Xafology.UnitTests.Import
{
    [TestFixture]
    public class ImportTests : Xafology.UnitTests.Import.ImportTestsBase
    {
        [Test]
        public void ValidateObjectTypeName()
        {
            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.ObjectTypeName = "InvalidObject";
            Assert.IsNull(param.ObjectTypeInfo);

            param.ObjectTypeName = "MockLookupObject1";
            Assert.AreEqual(typeof(MockLookupObject1), param.ObjectTypeInfo.Type);
        }

        [Test]
        public void CreateFieldMapsFromStream()
        {
            var csvText = @"Description,Amount,MockLookupObject1,MockLookupObject2
Hello 1,10,Parent 1,Parent B1
Hello 2,11,Parent 2,Parent B2
Hello 3,12,Parent 3,Parent B3
Hello 4,13,Parent 4,Parent B4
";
            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.ObjectTypeName = "MockFactObject";

            var csvStream = StringUtils.ConvertToCsvStream(csvText);

            var mapCreator = new FieldMapListCreator(csvStream);
            var fieldMaps = param.HeaderToFieldMaps;

            mapCreator.AppendFieldMaps(ObjectSpace.Session, fieldMaps);

            Assert.AreEqual(4, fieldMaps.Count);
        }

        public void CreateCsvTemplateFromObjectTypeInfo()
        {

        }

        // TODO: Test result of invalid type conversions
        //[Test]
        public void InvalidMemberValueConversions()
        {
            var xpoFieldMapper = new XpoFieldMapper(Application);

            var targetObject = ObjectSpace.CreateObject<MockFactObject>();

            var descMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject)).FindMember("Description");
            var amountMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject)).FindMember("Amount");
            var lookupMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockFactObject)).FindMember("MockLookupObject1");

            xpoFieldMapper.SetMemberValue(targetObject, descMember,
                    "Hello");
            xpoFieldMapper.SetMemberValue(targetObject, amountMember,
                    "15");
            xpoFieldMapper.SetMemberValue(targetObject, lookupMember,
                    "ABC");

            foreach (var obj in xpoFieldMapper.LookupsNotFound)
            {
                foreach (var value in obj.Value)
                    Debug.WriteLine("{0} {1}", obj.Key, value);
            }
        }
    }
}
