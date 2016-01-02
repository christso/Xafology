using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
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
    public class ImportViewController : ViewController
    {
        public const string ExecuteImportCaption = "Execute";
        public const string RemapCaption = "Remap";
        public const string TemplateCaption = "Template";

        public ImportViewController()
        {
            TargetObjectType = typeof(ImportParamBase);

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

        private void ImportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem.Caption)
            {
                case ExecuteImportCaption:
                    DoImport(this, EventArgs.Empty);
                    break;
                case RemapCaption:
                    DoRemap(this, EventArgs.Empty);
                    break;
                case TemplateCaption:
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
