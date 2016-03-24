using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Controllers
{
    /// <summary>
    /// Shows a popup window with a list of import profiles applicable to the current object type
    /// </summary>
    public class ImportableViewController : ViewController
    {
        private const string selectProfileCaption = "Select Profile";
        private const string createHeadersProfileCaption = "Create Profile - Headers";
        private const string createOrdinalsProfileCaption = "Create Profile - Ordinals";

        public ImportableViewController()
        {
            this.TargetObjectType = typeof(IXpoImportable);

            var importAction = new SingleChoiceAction(this, "ContextImportAction", PredefinedCategory.Edit);
            importAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            importAction.Caption = "Import";
            importAction.Execute += importAction_Execute;

            var runActionItem = new ChoiceActionItem();
            runActionItem.Caption = selectProfileCaption;
            importAction.Items.Add(runActionItem);

            var createHeadersActionItem = new ChoiceActionItem();
            createHeadersActionItem.Caption = createHeadersProfileCaption;
            importAction.Items.Add(createHeadersActionItem);

            var createOrdinalsActionItem = new ChoiceActionItem();
            createOrdinalsActionItem.Caption = createOrdinalsProfileCaption;
            importAction.Items.Add(createOrdinalsActionItem);

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void importAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case selectProfileCaption:
                    SelectProfile(sender, e);
                    break;
                case createHeadersProfileCaption:
                    CreateHeadersProfile(sender, e);
                    break;
                case createOrdinalsProfileCaption:
                    CreateOrdinalsProfile(sender, e);
                    break;
            }

        }

        private void SelectProfile(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            // create list view and filter by ObjectTypeName
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            var collectionSource = new CollectionSource(objectSpace, typeof(ImportParamBase));
            collectionSource.Criteria["ObjectTypeFilter"] = CriteriaOperator.Parse("ObjectTypeName == ?", View.ObjectTypeInfo.Name);
            var listViewId = Application.FindListViewId(typeof(ImportParamBase));
            var listView = Application.CreateListView(
                listViewId,
                collectionSource,
                true);

            // show view in new window
            var svp = e.ShowViewParameters;
            svp.TargetWindow = TargetWindow.NewWindow;
            svp.CreatedView = listView;
        }

        private void CreateHeadersProfile(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            var newProfile = objectSpace.CreateObject<ImportHeadersParam>();
            newProfile.ObjectTypeName = View.ObjectTypeInfo.Type.Name;
            var detailView = Application.CreateDetailView(
                objectSpace,
                newProfile);
            var svp = e.ShowViewParameters;
            svp.TargetWindow = TargetWindow.NewWindow;
            svp.CreatedView = detailView;
        }

        private void CreateOrdinalsProfile(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            var newProfile = objectSpace.CreateObject<ImportOrdinalsParam>();
            newProfile.ObjectTypeName = View.ObjectTypeInfo.Type.Name;
            var detailView = Application.CreateDetailView(
                objectSpace,
                newProfile);
            var svp = e.ShowViewParameters;
            svp.TargetWindow = TargetWindow.NewWindow;
            svp.CreatedView = detailView;
        }
    }
}
