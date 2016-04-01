using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.PivotGrid.Web;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;
using Xafology.ExpressApp.PivotGridLayout;

using Xafology.ExpressApp.PivotGridLayout.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.PivotGrid;
using DevExpress.Xpo;
using Xafology.ExpressApp.PivotGridLayout.Controllers;

namespace Xafology.ExpressApp.PivotGridLayout.Web.Controllers
{

    public class PivotGridLayoutControllerWeb : PivotGridLayoutController
    {
        /// <summary>
        /// Called each time that fields are mapped for a layout.
        /// </summary>
        public event PivotGridLayoutFieldsMappedEventHandler PivotGridFieldsMapped;

        private ASPxPivotGrid _PivotGridControl;
        private bool IsPivotGridLayoutLoading = false;

        public ASPxPivotGrid PivotGridControl
        {
            get
            {
                return _PivotGridControl;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            IsPivotGridLayoutLoading = true;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var pgEditor = (ASPxPivotGridListEditor)((ListView)View).Editor;
            if (pgEditor != null)
                _PivotGridControl = (ASPxPivotGrid)pgEditor.PivotGridControl;
            
            _PivotGridControl.BeginRefresh += _PivotGridControl_BeginRefresh;
        }

        void _PivotGridControl_BeginRefresh(object sender, EventArgs e)
        {
            if (IsPivotGridLayoutLoading)
            {
                var loadedLayoutObj = FindLoadedLayout(ObjectSpace, UIPlatform.Web, View.ObjectTypeInfo.Type.Name);

                if (loadedLayoutObj != null && loadedLayoutObj.SavedLayout != null)
                {
                    LoadStartupPivotGridLayout(loadedLayoutObj.SavedLayout);
                }
            }
            IsPivotGridLayoutLoading = false;
            _PivotGridControl.BeginRefresh -= _PivotGridControl_BeginRefresh;
        }

        private MemoryStream _CachedStream;

        protected override void CacheLayoutStream()
        {
            base.CacheLayoutStream();
            var stream = new MemoryStream();
            PivotGridControl.SaveLayoutToStream(stream);
            stream.Position = 0;
            _CachedStream = stream;
        }

        protected override void SavePivotGridLayout(PivotGridSavedLayout savedLayoutObj)
        {
            base.SavePivotGridLayout(savedLayoutObj);

            // Open stream.
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                PivotGridControl.SaveLayoutToStream(stream); // potentially throws a null exception
                stream.Position = 0;
            }
            catch
            {
                // cache the stream for deferred loading if the control object is not available
                stream = _CachedStream;
            }

            // Save stream to datastore.
            savedLayoutObj.LayoutFile = new FileData(savedLayoutObj.Session);
            savedLayoutObj.LayoutFile.LoadFromStream("PivotGridLayout.xml", stream);
            savedLayoutObj.UIPlatform = UIPlatform.Web;
            savedLayoutObj.TypeName = TargetTypeName;
            savedLayoutObj.Session.CommitTransaction();

            // Save to Loaded Layout
            var session = savedLayoutObj.Session;
            var loadedLayoutObj = FindLoadedLayout(session, UIPlatform.Web, TargetTypeName);
            if (loadedLayoutObj == null)
                loadedLayoutObj = new PivotGridLoadedLayout(session);
            loadedLayoutObj.SavedLayout = savedLayoutObj;
            session.CommitTransaction();
            UpdateLayoutActionCaption(savedLayoutObj);
        }

        protected void LoadStartupPivotGridLayout(PivotGridSavedLayout savedLayoutObj)
        {
            PivotGridControl.Fields.Clear();
            var stream = new MemoryStream(savedLayoutObj.LayoutFile.Content);
            stream.Position = 0;
            PivotGridControl.LoadLayoutFromStream(stream);
            UpdateLayoutActionCaption(savedLayoutObj);
        }

        protected override void LoadPivotGridLayout(PivotGridSavedLayout savedLayoutObj)
        {
            PivotGridControl.Fields.Clear();

            IsPivotGridLayoutLoading = true;

            if (savedLayoutObj.UIPlatform != UIPlatform.Web
               && savedLayoutObj.UIPlatform != UIPlatform.All)
                throw new ArgumentException("Cannot load layout that is built for another UI platform.");
            if (savedLayoutObj.TypeName != TargetTypeName)
                throw new ArgumentException("Cannot load layout that is built for another object type.");

            var session = savedLayoutObj.Session;
            var loadedLayoutObj = FindLoadedLayout(session, UIPlatform.Web, TargetTypeName);
            if (loadedLayoutObj == null)
            {
                loadedLayoutObj = new PivotGridLoadedLayout(session);
                loadedLayoutObj.SavedLayout = savedLayoutObj;
            }
            loadedLayoutObj.SavedLayout = savedLayoutObj;
            session.CommitTransaction();
            UpdateLayoutActionCaption(savedLayoutObj);
        }

        protected override void ResetPivotGridLayouts(PivotGridSetup pivotSetup)
        {
            foreach (var layout in pivotSetup.Layouts)
            {
                PivotGridControl.Fields.Clear();

                layout.LayoutFields();

                // get PivotGridSavedLayout object
                PivotGridSavedLayout savedLayoutObj;
                savedLayoutObj = ObjectSpace.FindObject<PivotGridSavedLayout>(CriteriaOperator.Parse(
                    "LayoutName = ? And UIPlatform = ? And TypeName = ?",
                    layout.Name, UIPlatform.Web, TargetTypeName));
                if (savedLayoutObj == null)
                {
                    savedLayoutObj = ObjectSpace.CreateObject<PivotGridSavedLayout>();
                    savedLayoutObj.LayoutName = layout.Name;
                }

                // map PivotGridFieldBase to PivotGridField
                foreach (var bf in pivotSetup.Fields)
                {
                    var webField = Utils.MapPivotGridFieldToWeb(bf);
                    PivotGridControl.Fields.Add(webField);
                }
                if (PivotGridFieldsMapped != null)
                    PivotGridFieldsMapped(this, new PivotGridLayoutEventArgs() { Layout = layout });

                // Save layout to persistent object
                SavePivotGridLayout(savedLayoutObj);
            }
        }

        public override CriteriaOperator SavedLayoutUICriteria
        {
            get
            {
                return CriteriaOperator.Parse(
                    "UIPlatform = ? And TypeName = ?", UIPlatform.Web, TargetTypeName);
            }
        }

        private string TargetTypeName
        {
            get
            {
                return View.ObjectTypeInfo.Name;
            }
        }
    }
}
