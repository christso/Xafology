using Xafology.ExpressApp.PivotGridLayout.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;

namespace Xafology.ExpressApp.PivotGridLayout.Controllers
{
    public delegate void PivotGridLayoutEventHandler(object sender);
    public delegate void PivotGridLayoutFieldsMappedEventHandler(object sender, PivotGridLayoutEventArgs e);

    public class PivotGridLayoutController : ViewController
    {
        public event PivotGridLayoutEventHandler PivotGridLayoutReset;


        public PivotGridSetup PivotGridSetupObject;
        public readonly string DefaultLayoutActionCaption = "Layout";
        private readonly SingleChoiceAction _LayoutAction = null;

        public PivotGridLayoutController()
        {
            TargetViewType = ViewType.ListView;

            // unique view ID so that it will not match any existing views
            // unless the developer assigns it another View ID in a derived constructor
            TargetViewId = "{44F8DE78-5DBB-4316-AFDC-8F8A58D4E2FC}";

            _LayoutAction = new SingleChoiceAction(this, "LayoutAction", DevExpress.Persistent.Base.PredefinedCategory.View);
            _LayoutAction.Caption = DefaultLayoutActionCaption;
            _LayoutAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            _LayoutAction.ShowItemsOnClick = true;
            _LayoutAction.Execute += myLayoutAction_Execute;

            var resetLayoutChoice = new ChoiceActionItem();
            resetLayoutChoice.Caption = "Reset";
            _LayoutAction.Items.Add(resetLayoutChoice);

            var saveLayoutChoice = new ChoiceActionItem();
            saveLayoutChoice.Caption = "Save";
            _LayoutAction.Items.Add(saveLayoutChoice);

            var loadLayoutChoice = new ChoiceActionItem();
            loadLayoutChoice.Caption = "Load";
            _LayoutAction.Items.Add(loadLayoutChoice);

            //var testChoice = new ChoiceActionItem();
            //testChoice.Caption = "Test";
            //_LayoutAction.Items.Add(testChoice);

        }

        public SingleChoiceAction LayoutAction
        {
            get
            {
                return _LayoutAction;
            }
        }

        protected void UpdateLayoutActionCaption(PivotGridSavedLayout savedLayoutObj)
        {
            //LayoutAction.Caption = String.Format("{0}: {1}", DefaultLayoutActionCaption, savedLayoutObj.LayoutName);
            View.Caption = View.ObjectTypeInfo.Name + " - " + savedLayoutObj.LayoutName;
        }

        /// <summary>
        /// used by ASP.NET to cache the PivotGrid XML stream
        /// and only load the stream to the control when the control is refreshed.
        /// </summary>
        protected virtual void CacheLayoutStream()
        {
        }

        private void myLayoutAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (e.SelectedChoiceActionItem.Caption == "Reset")
            {
                ResetPivotGridLayout();
            }
            else if (e.SelectedChoiceActionItem.Caption == "Save")
            {
                CacheLayoutStream();

                IObjectSpace objectSpace = Application.CreateObjectSpace();

                Type objType = typeof(PivotGridSavedLayout);
                string viewId = "PivotGridSavedLayoutUISave_ListView";
                var collectionSource = Application.CreateCollectionSource(Application.CreateObjectSpace(), objType, viewId);
                collectionSource.Criteria["UIFilter"] = SavedLayoutUICriteria;
                ListView listView = Application.CreateListView(viewId, collectionSource, false);
                ShowViewParameters svp = e.ShowViewParameters;
                svp.TargetWindow = TargetWindow.NewModalWindow;

                var pc = new SavedLayoutPopupListViewController();
                svp.Controllers.Add(pc);
                pc.SendingViewController = this;

                var dc = new DialogController();
                dc.Accepting += saveDialog_Accepting;
                svp.Controllers.Add(dc);

                svp.CreatedView = listView;
               
            }
            else if (e.SelectedChoiceActionItem.Caption == "Load")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                Type objType = typeof(PivotGridSavedLayout);
                string viewId = "PivotGridSavedLayoutUILoad_ListView";
                var collectionSource = Application.CreateCollectionSource(Application.CreateObjectSpace(), objType, viewId);
                collectionSource.Criteria["UIFilter"] = SavedLayoutUICriteria;
                ListView listView = Application.CreateListView(viewId, collectionSource, true);
                var svp = e.ShowViewParameters;
                svp.TargetWindow = TargetWindow.NewModalWindow;

                var dc = new DialogController();
                dc.Accepting += loadDialog_Accepting;
                svp.Controllers.Add(dc);
                svp.CreatedView = listView;
            }
        }

        protected virtual void ResetPivotGridLayouts(PivotGridSetup pivotSetup)
        {
        }

        void saveDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var savedLayoutObj = (PivotGridSavedLayout)e.AcceptActionArgs.CurrentObject;
            if (savedLayoutObj != null)
                SavePivotGridLayout(savedLayoutObj);
        }
        private void loadDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var savedLayoutObj = (PivotGridSavedLayout)e.AcceptActionArgs.CurrentObject;
            if (savedLayoutObj != null)
                LoadPivotGridLayout(savedLayoutObj);
        }

        protected virtual void ResetPivotGridLayout()
        {
            LayoutAction.Caption = DefaultLayoutActionCaption;
            ResetPivotGridLayouts(PivotGridSetupObject);
            if (PivotGridLayoutReset != null)
                PivotGridLayoutReset(this);
        }

        protected virtual void SavePivotGridLayout(PivotGridSavedLayout layoutObj)
        {
        }

        protected virtual void LoadPivotGridLayout(PivotGridSavedLayout layoutObj)
        {
        }

        public static PivotGridLoadedLayout FindLoadedLayout(IObjectSpace os, UIPlatform platform, string typeName)
        {
            return FindLoadedLayout(((XPObjectSpace)os).Session, platform, typeName);
        }

        public static PivotGridLoadedLayout FindLoadedLayout(Session session, UIPlatform platform, string typeName)
        {
            var currentUser = StaticHelpers.GetCurrentUser(session);
            CriteriaOperator criteria = null;
            if (currentUser == null)
                criteria = CriteriaOperator.Parse(
                 "UIPlatform = ? And User Is Null And TypeName = ?", platform, typeName);
            else
                criteria = CriteriaOperator.Parse(
                 "UIPlatform = ? And User = ? And TypeName = ?", platform, currentUser, typeName);
            var loadedLayoutObj = session.FindObject<PivotGridLoadedLayout>(criteria);
            return loadedLayoutObj;
        }

        public virtual CriteriaOperator SavedLayoutUICriteria
        {
            get
            {
                return null;
            }
        }
    }
}
