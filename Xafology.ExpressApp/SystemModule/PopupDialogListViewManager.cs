using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;

namespace Xafology.ExpressApp.SystemModule
{
    public class PopupDialogListViewManager
    {
        private XafApplication _app;
        private ShowViewParameters _svp;
        private Type _objType;
        private DialogController _dc;
        private IObjectSpace _objSpace;

        public event EventHandler<DialogControllerAcceptingEventArgs> Accepting;


        public PopupDialogListViewManager(XafApplication app, string viewId, IObjectSpace objSpace)
        {
            _objSpace = objSpace;
        }


        public PopupDialogListViewManager(XafApplication app, Type objType, IObjectSpace objSpace)
            : this(app, objType)
        {
            _objSpace = objSpace;
        }

        public PopupDialogListViewManager(XafApplication app, Type objType)
        {
            _app = app;
            _objType = objType;
            _dc = app.CreateController<DialogController>();
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
        // TODO: check if collectionsource is the right way to create list view
        public void ShowView(string listViewId)
        {
            var objSpace = _objSpace ?? _app.CreateObjectSpace();
            _svp = new ShowViewParameters();
            var collectionSource = new CollectionSource(objSpace, _objType);
            _svp.CreatedView = _app.CreateListView(listViewId, collectionSource, true);

            _svp.TargetWindow = TargetWindow.NewModalWindow;
            _svp.Context = TemplateContext.PopupWindow;
            _svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            _svp.Controllers.Add(_dc);

            _app.ShowViewStrategy.ShowView(_svp, new ShowViewSource(null, null));
        }

        public void ShowView(string listViewId, CriteriaOperator criteria)
        {
            var objSpace = _objSpace ?? _app.CreateObjectSpace();
            _svp = new ShowViewParameters();
            var collectionSource = new CollectionSource(objSpace, _objType);
            _svp.CreatedView = _app.CreateListView(listViewId, collectionSource, true);

            _svp.TargetWindow = TargetWindow.NewModalWindow;
            _svp.Context = TemplateContext.PopupWindow;
            _svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            _svp.Controllers.Add(_dc);

            ((ListView)_svp.CreatedView).CollectionSource.Criteria["NewCriteria"] = criteria;
            _app.ShowViewStrategy.ShowView(_svp, new ShowViewSource(null, null));
        }

        public void ShowView()
        {
            var objSpace = _objSpace ?? _app.CreateObjectSpace();
            _svp = new ShowViewParameters();
            _svp.CreatedView = _app.CreateListView(objSpace,
                _objType, false);

            _svp.TargetWindow = TargetWindow.NewModalWindow;
            _svp.Context = TemplateContext.PopupWindow;
            _svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            _svp.Controllers.Add(_dc);
            _app.ShowViewStrategy.ShowView(_svp, new ShowViewSource(null, null));

        }

        public void ShowView(CriteriaOperator criteria)
        {
            var objSpace = _objSpace ?? _app.CreateObjectSpace();
            _svp = new ShowViewParameters();
            _svp.CreatedView = _app.CreateListView(objSpace,
                _objType, false);

            _svp.TargetWindow = TargetWindow.NewModalWindow;
            _svp.Context = TemplateContext.PopupWindow;
            _svp.CreateAllControllers = true;

            if (Accepting != null)
                _dc.Accepting += DialogController_Accepting;

            _svp.Controllers.Add(_dc);

            ((ListView)_svp.CreatedView).CollectionSource.Criteria["NewCriteria"] = criteria;
            _app.ShowViewStrategy.ShowView(_svp, new ShowViewSource(null, null));

        }

        void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            Accepting(sender, e);
        }
    }
}
