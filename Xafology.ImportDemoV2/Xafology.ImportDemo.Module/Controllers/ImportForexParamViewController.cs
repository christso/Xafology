using Xafology.ImportDemo.Module.ParamObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;

namespace Xafology.ImportDemo.Module.Controllers
{
    public class ImportForexParamViewController : ViewController<DetailView>
    {
        public ImportForexParamViewController()
        {
            TargetObjectType = typeof(ImportForexRatesParam);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
            var dc = Frame.GetController<DialogController>();
            if (dc != null)
                dc.AcceptAction.Execute += AcceptAction_Execute;
        }

        private void AcceptAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {

        }
    }
}
