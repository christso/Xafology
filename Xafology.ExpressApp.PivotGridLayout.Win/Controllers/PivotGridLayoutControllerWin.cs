using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.PivotGrid.Win;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraPivotGrid;
using Xafology.ExpressApp.PivotGridLayout;
using Xafology.ExpressApp.PivotGridLayout.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.PivotGrid;
using Xafology.ExpressApp.PivotGridLayout.Controllers;

namespace Xafology.ExpressApp.PivotGridLayout.Win.Controllers
{
    public class PivotGridLayoutControllerWin : PivotGridLayoutController
    {
        /// <summary>
        /// Called each time that fields are mapped for a layout.
        /// </summary>
        public event PivotGridLayoutFieldsMappedEventHandler PivotGridFieldsMapped;
        private PivotGridControl _PivotGridControl;

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        public PivotGridControl PivotGridControl
        {
            get
            {
                return _PivotGridControl;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var pgEditor = ((ListView)View).Editor as PivotGridListEditor;
            if (pgEditor != null)
                _PivotGridControl = pgEditor.PivotGridControl;

            var loadedLayoutObj = FindLoadedLayout(ObjectSpace, UIPlatform.Win, TargetTypeName);
            if (loadedLayoutObj != null && loadedLayoutObj.SavedLayout != null)
            {
                LoadPivotGridLayout(loadedLayoutObj.SavedLayout);
            }
        }
  
        protected override void SavePivotGridLayout(PivotGridSavedLayout savedLayoutObj)
        {
            base.SavePivotGridLayout(savedLayoutObj);

            // Open stream.
            var stream = new MemoryStream();
            PivotGridControl.SaveLayoutToStream(stream);
            stream.Position = 0;

            // Save stream to datastore.
            savedLayoutObj.LayoutFile = new FileData(savedLayoutObj.Session);
            savedLayoutObj.LayoutFile.LoadFromStream("PivotGridLayout.xml", stream);
            savedLayoutObj.UIPlatform = UIPlatform.Win;
            savedLayoutObj.TypeName = TargetTypeName;
            savedLayoutObj.Session.CommitTransaction();

            // Save to Loaded Layout
            var session = savedLayoutObj.Session;
            var loadedLayoutObj = FindLoadedLayout(session, UIPlatform.Win, View.ObjectTypeInfo.Type.Name);
            if (loadedLayoutObj == null)
                loadedLayoutObj = new PivotGridLoadedLayout(session);
            loadedLayoutObj.SavedLayout = savedLayoutObj;
            session.CommitTransaction();
            UpdateLayoutActionCaption(savedLayoutObj);
        }

        protected override void LoadPivotGridLayout(PivotGridSavedLayout savedLayoutObj)
        {
            PivotGridControl.Fields.Clear();

            if (savedLayoutObj.UIPlatform != UIPlatform.Win
                && savedLayoutObj.UIPlatform != UIPlatform.All)
                throw new ArgumentException("Cannot load layout that is built for another UI platform.");
            if (savedLayoutObj.TypeName != View.ObjectTypeInfo.Type.Name)
                throw new ArgumentException("Cannot load layout that is built for another object type.");

            var stream = new MemoryStream(savedLayoutObj.LayoutFile.Content);
            stream.Position = 0;
            PivotGridControl.RestoreLayoutFromStream(stream);

            var session = savedLayoutObj.Session;
            var loadedLayoutObj = FindLoadedLayout(session, UIPlatform.Win, TargetTypeName);
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
            if (pivotSetup == null)
                throw new ArgumentNullException("pivotSetup");

            foreach (var layout in pivotSetup.Layouts)
            {
                PivotGridControl.Fields.Clear();

                layout.LayoutFields();

                // get PivotGridSavedLayout object
                PivotGridSavedLayout savedLayoutObj;
                savedLayoutObj = ObjectSpace.FindObject<PivotGridSavedLayout>(CriteriaOperator.Parse(
                    "LayoutName = ? And UIPlatform = ? And TypeName = ?",
                    layout.Name, UIPlatform.Win, TargetTypeName));
                if (savedLayoutObj == null)
                {
                    savedLayoutObj = ObjectSpace.CreateObject<PivotGridSavedLayout>();
                    savedLayoutObj.LayoutName = layout.Name;
                }

                // map PivotGridFieldBase to PivotGridField
                foreach (var bf in pivotSetup.Fields)
                {
                    var winField = Utils.MapPivotGridFieldToWin(bf);
                    PivotGridControl.Fields.Add(winField);
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
                    "UIPlatform = ? And TypeName = ?", UIPlatform.Win, TargetTypeName);
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
