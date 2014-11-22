using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;

namespace Xafology.ExpressApp.Controllers
{
    public abstract class DefaultValuesViewController : ViewController
    {
        private Dictionary<string, object> _defaultValues;
        public Dictionary<string, object> DefaultValues
        {
            get { return _defaultValues; }
            set { _defaultValues = value; }
        }

        private SimpleAction newObjectAction;

        public DefaultValuesViewController(Dictionary<string, object> defaultValues)
        {
            TargetObjectType = null;
            _defaultValues = defaultValues;
            newObjectAction = new SimpleAction(this, "New", "ObjectsCreation");
            newObjectAction.ImageName = "MenuBar_New";
            newObjectAction.Shortcut = "CtrlN";
            newObjectAction.Execute += newObjectAction_Execute;
        }

        void newObjectAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objSpace = Application.CreateObjectSpace();
            var obj = objSpace.CreateObject(View.ObjectTypeInfo.Type);
            InitializeObjectValues(obj);
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objSpace, obj);
            e.ShowViewParameters.TargetWindow = TargetWindow.Current;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreating += newObjectViewController_ObjectCreating;
        }

        protected abstract void InitializeObjectValues(object currentObject);

        void newObjectViewController_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            var detailView = View as DetailView;
            if (detailView != null && detailView.ViewEditMode == ViewEditMode.Edit
                && _defaultValues != null)
            {
                InitializeObjectValues(View.CurrentObject);
            }
        }

    }
}
