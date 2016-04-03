using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteDemo.Module.Win.Controllers
{
    public class PasteTestController : ViewController<ListView>
    {
        private const string test1Caption = "Test1";
        public PasteTestController()
        {
            var testPasteAction = new SingleChoiceAction(this, "TestPasteAction", PredefinedCategory.ObjectsCreation);
            testPasteAction.Caption = "Test";
            testPasteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            testPasteAction.Execute += testPasteAction_Execute;
            var Test1 = new ChoiceActionItem();
            Test1.Caption = "Test1";
            testPasteAction.Items.Add(Test1);
        }

        private void testPasteAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case test1Caption:
                    Test1(e);
                    break;
            }
        }

        private void Test1(SimpleActionExecuteEventArgs e)
        {
            var listEditor = View.Editor as GridListEditor;

            var columns1 = View.Model.Columns;
            var columns2 = listEditor.Model.Columns;
        }
    }
}
