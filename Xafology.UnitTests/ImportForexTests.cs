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
using System.Reflection;

namespace Xafology.ImportDemo.UnitTests
{
    public class ImportForexTests : ImportTestsBase
    {
        private const string ForexRateResourcePath = "Xafology.ImportDemo.UnitTests.Resources.GLXR140424.txt";

        public ImportForexTests()
        {
            SetTesterDbType(TesterDbType.MsSql);
        }

        [Test]
        public void GetTestStream()
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(ForexRateResourcePath);
            Assert.NotNull(stream);
        }

        [Test]
        public void ImportForexRateTextFile()
        {
            var xpoMapper = new XpoFieldMapper(Application);
            var param = ObjectSpace.CreateObject<ImportForexParam>();

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                ForexRateResourcePath);


            stream.Position = 0;
            var loader = new Xafology.ImportDemo.UnitTests.ForexRateInserter(param, stream, xpoMapper);
            
            loader.Execute();
        }


    }
}
