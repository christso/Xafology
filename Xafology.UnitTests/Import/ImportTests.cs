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

namespace Xafology.UnitTests.Import
{
    [TestFixture]
    public class ImportTests : Xafology.UnitTests.Import.ImportTestsBase
    {
        [Test]
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

        [Test]
        public void CacheTest()
        {
            // add objects to cache dictionary
            var obj1 = new MockFactObject(ObjectSpace.Session) { Description = "A", Amount = 10 };
            var obj2 = new MockFactObject(ObjectSpace.Session) { Description = "B", Amount = 20 };
            ObjectSpace.CommitChanges();

            var objs = new XPCollection(ObjectSpace.Session, typeof(MockFactObject));

            var q = new XPQuery<MockFactObject>(ObjectSpace.Session);

            Debug.Print(string.Format("{0}", q.Count()));
            Debug.Print(string.Format("{0}", q.Count()));

            foreach (var obj in q)
            {
                Debug.Print(((MockFactObject)obj).Description);
            }

            foreach (var obj in objs)
            {
                Debug.Print(((MockFactObject)obj).Description);
            }
        }
    }
}
