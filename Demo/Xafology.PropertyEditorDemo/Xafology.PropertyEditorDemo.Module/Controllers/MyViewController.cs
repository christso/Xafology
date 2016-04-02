using Xafology.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using System;

namespace Xafology.PropertyEditorDemo.Module.Controllers
{
    public class MyViewController : ViewController<DetailView>
    {
        public MyViewController()
        {
            //var calculateAction = new SimpleAction(this, "CalculateAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            //calculateAction.Caption = "Calculate";
            //calculateAction.Execute += calculateAction_Execute;
        }

        //void calculateAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    var editor = (PropertyEditor)sender;
        //    editor.PropertyValue = Convert.ToDecimal(1000);
        //}

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var viewItems = View.GetItems<IActionPropertyEditor>();
            foreach (var viewItem in viewItems)
            {
                var editor = ((PropertyEditor)viewItem);
                if (editor.Id == "Amount")
                    viewItem.ButtonClick += amountViewItem_ButtonClick;
            }
        }

        void amountViewItem_ButtonClick(PropertyEditor sender, ActionPropertyClickEventArgs e)
        {
            sender.PropertyValue = Convert.ToDecimal(1000);
            sender.Refresh();
        }
    }
}
