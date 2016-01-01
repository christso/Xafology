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

namespace Xafology.UnitTests
{
    [TestFixture]
    public class ImportTests : ImportTestsBase
    {
        [Test]
        public void TestMemberValues()
        {
            var obj = ObjectSpace.CreateObject<MockImportObject>();

            var typeInfo = XafTypesInfo.Instance.FindTypeInfo(obj.GetType());
            var members = typeInfo.Members;
            foreach (var member in members)
            {
                Console.WriteLine(member.Name);
            }
        }
    }
}
