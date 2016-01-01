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
using Xafology.ExpressApp;
using System.Diagnostics;

namespace Xafology.UnitTests
{
    [TestFixture]
    public class ImportTests : ImportTestsBase
    {
        [Test]
        public void InvalidMemberValueConversions()
        {
            var xpoFieldMapper = new XpoFieldMapper(Application);

            var targetObject = ObjectSpace.CreateObject<MockImportObject>();

            var descMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockImportObject)).FindMember("Description");
            var amountMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockImportObject)).FindMember("Amount");
            var lookupMember = XafTypesInfo.Instance.FindTypeInfo(typeof(MockImportObject)).FindMember("MockLookupObject");

            xpoFieldMapper.SetMemberValue(targetObject, descMember,
                    "Hello");
            xpoFieldMapper.SetMemberValue(targetObject, amountMember,
                    "15");
            xpoFieldMapper.SetMemberValue(targetObject, lookupMember,
                    "ABC");

            foreach (var obj in xpoFieldMapper.XpObjectsNotFound)
            {
                foreach (var value in obj.Value)
                    Debug.WriteLine("{0} {1}", obj.Key, value);
            }

            
        }
    }
}
