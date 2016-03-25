using NUnit.Framework;
using PasteDemo.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.TestUtils;

namespace PasteDemo.UnitTests
{
    [TestFixture]
    public class PasteTests : TestBase
    {
        #region Setup
        public PasteTests()
        {
            SetTesterDbType(TesterDbType.InMemory);
        }

        public override void OnAddExportedTypes(DevExpress.ExpressApp.ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(MockFactObject));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject1));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject2));
        }
        #endregion

        #region Tests

        [Test]
        public void PasteText()
        {
            string copiedText = "qwerwer\t2000\tApple\tHandset Pchse\r\n";

        }

        #endregion
    }
}
