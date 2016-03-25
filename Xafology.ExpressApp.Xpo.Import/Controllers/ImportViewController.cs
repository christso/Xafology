using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.Utils;

namespace Xafology.ExpressApp.Xpo.Import.Controllers
{
    /// <summary>
    /// Executes the import process for the selected object type
    /// </summary>
    public class ImportViewController : ViewController
    {
        public const string ExecuteImportCaption = "Execute";
        public const string RemapCaption = "Remap";
        public const string TemplateCaption = "Template";

        public ImportViewController()
        {
            TargetObjectType = typeof(ImportParamBase);
            TargetViewType = ViewType.DetailView;

            // action button

            var importAction = new SingleChoiceAction(this, "ImportAction", PredefinedCategory.ObjectsCreation);
            importAction.DefaultItemMode = DefaultItemMode.FirstActiveItem;
            importAction.Caption = "Import";
            importAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            importAction.Execute += ImportAction_Execute;

            var executeAction = new ChoiceActionItem();
            executeAction.Caption = ExecuteImportCaption;
            importAction.Items.Add(executeAction);

            var remapAction = new ChoiceActionItem();
            remapAction.Caption = RemapCaption;
            importAction.Items.Add(remapAction);

            var templateAction = new ChoiceActionItem();
            templateAction.Caption = TemplateCaption;
            importAction.Items.Add(templateAction);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            newObjectViewController.ObjectCreated += NewObjectViewController_ObjectCreated;
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            var newValue = e.NewValue;

            var obj = e.Object as ImportParamBase;

            if (obj != null
                && e.PropertyName == ImportParamBase.Fields.CacheLookupObjects.PropertyName
                && e.NewValue != null)
            {
                foreach (var map in obj.FieldMaps)
                {
                    map.CacheObject = (bool)e.NewValue;
                }
            }
        }

        // set default values in newly created object
        private void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {

            var param = (ImportParamBase)e.CreatedObject;
            //param.ObjectTypeName = objTypeName;
        }

        // Types of actions available to the user
        private void ImportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case ExecuteImportCaption:
                    // execute the import into datastore
                    DoImport(this, EventArgs.Empty);
                    break;
                case RemapCaption:
                    // create mappings based on uploaded file and object type
                    DoRemap(this, EventArgs.Empty); 
                    break;
                case TemplateCaption:
                    // create template with accepted column names for upload file
                    DoTemplate(this, EventArgs.Empty);
                    break;
                default:
                    throw new UserFriendlyException(
                        string.Format("Choice '{0}' is not valid.", e.SelectedChoiceActionItem.Caption));
            }
        }
        public event EventHandler DoImport;
        public event EventHandler DoRemap;
        public event EventHandler DoTemplate;

    }
}
