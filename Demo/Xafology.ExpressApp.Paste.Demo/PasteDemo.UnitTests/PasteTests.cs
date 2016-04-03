using NUnit.Framework;
using Moq;
using PasteDemo.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.TestUtils;
using Xafology.ExpressApp.Paste.Win;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Model;

namespace PasteDemo.UnitTests
{
    [TestFixture]
    public class PasteTests : TestBase
    {
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

        [Test]
        public void ParseCopiedRow()
        {
            var mockCopiedText = new Mock<ICopiedText>();
            mockCopiedText.Setup(x => x.Data).Returns("some description\t2000\tApple\tHandset Pchse");

            var controller = new TestController();
            var cs = new CollectionSource(ObjectSpace, typeof(MockFactObject));
            var viewModel = (IModelListView)Application.FindModelView(Application.FindListViewId(typeof(MockFactObject)));
            viewModel.EditorType = typeof(TestListEditor);
            var listView = Application.CreateListView(viewModel, cs, true);
            controller.SetView(listView);

            var copyParser = new CopyParser(mockCopiedText.Object);
            var parsedArray = copyParser.ToArray();

            Assert.AreEqual("some description", parsedArray[0][0]);
            Assert.AreEqual("2000", parsedArray[0][1]);
            Assert.AreEqual("Apple", parsedArray[0][2]);
            Assert.AreEqual("Handset Pchse", parsedArray[0][3]);
        }
    }
}
