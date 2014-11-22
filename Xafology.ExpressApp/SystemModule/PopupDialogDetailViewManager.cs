using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;

namespace Xafology.ExpressApp.SystemModule
{
    public class PopupDialogDetailViewManager
    {
        private XafApplication _app;
        private ShowViewParameters _svp;
        private DialogController _dc;

        public event EventHandler<DialogControllerAcceptingEventArgs> Accepting;

        public PopupDialogDetailViewManager(XafApplication app)
        {
            _app = app;
            _dc = app.CreateController<DialogController>();
        }

        public DialogController DialogController
        {
            get
            {
                return _dc;
            }
        }

        public bool CanCloseWindow
        {
            get
            {
                return _dc.CanCloseWindow;
            }
            set
            {
                _dc.CanCloseWindow = value;
            }
        }

        public void ShowNonPersistentView(IObjectSpace objSpace, Type objType)
        {
            if (objType == null)
                throw new NullReferenceException("Object Type must not be null if you want to call ShowNonPersistentView");
            objSpace = objSpace ?? ObjectSpaceInMemory.CreateNew();
            object obj = Activator.CreateInstance(objType);
            ShowView(objSpace, obj);
        }

        public void ShowNonPersistentView(Type objType)
        {
            ShowNonPersistentView(null, objType);
        }

        /// <summary>
        /// Show Non-Persistent Object
        /// </summary>
        public void ShowNonPersistentView(object obj)
        {
            ShowView(ObjectSpaceInMemory.CreateNew(), obj);
        }

        public void ShowView(IObjectSpace objSpace, object obj)
        {
            _svp = new ShowViewParameters();
            _svp.CreatedView = _app.CreateDetailView(objSpace, obj);
            _svp.TargetWindow = TargetWindow.NewModalWindow;
            _svp.Context = TemplateContext.PopupWindow;
            _svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            _svp.Controllers.Add(_dc);
            _app.ShowViewStrategy.ShowView(_svp, new ShowViewSource(null, null));
        }

        void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            Accepting(sender, e);
        }

        public void ShowSingletonView<T>(IObjectSpace objSpace)
        {
            var svp = new ShowViewParameters();
            svp.CreatedView = _app.CreateDetailView(objSpace,
                StaticHelpers.GetInstance<T>(objSpace));
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.PopupWindow;
            svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            svp.Controllers.Add(_dc);
            _app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public void ShowSingletonView<T>()
        {
            ShowSingletonView<T>(_app.CreateObjectSpace());
        }



    }
}
