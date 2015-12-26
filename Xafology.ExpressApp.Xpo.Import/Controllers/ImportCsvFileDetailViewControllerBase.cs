using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using System.IO;

namespace Xafology.ExpressApp.Xpo.Import.Controllers
{

    public class ImportCsvFileDetailViewControllerBase : ViewController<DetailView>
    {
        public ImportCsvFileDetailViewControllerBase()
        {
            TargetObjectType = typeof(Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase);
            importLogic = null; // lazy loading
        }

        private Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileLogic importLogic;

        protected override void OnActivated()
        {
            base.OnActivated();
            ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
            var sourceView = Application.MainWindow.View;

            var dc = Frame.GetController<DialogController>();
            if (dc != null)
            {
                dc.AcceptAction.Execute += AcceptAction_Execute;
                dc.CancelAction.Execute += CancelAction_Execute;
                dc.CanCloseWindow = false;
            }
            var param = View.CurrentObject as Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase;
            if (param != null)
            {
                // create template for object type in main view
                Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileLogic.CreateTemplate(param, sourceView.ObjectTypeInfo);
            }
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (e.Object is IFileData && e.PropertyName == "Content")
            {
                // Create CSV Field Maps if user attaches file
                CreateCsvFieldMaps((Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase)View.CurrentObject);
            }
        }

        private void CreateCsvFieldMaps(Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase paramObj)
        {
            if (paramObj.File.Content == null)
                return;
            var byteArray = paramObj.File.Content;
            var stream = new MemoryStream(byteArray);

            importLogic = null;

            importLogic = paramObj.CreateImportLogic(Application, stream);
            if (paramObj.FieldImportMaps.Count == 0)
                importLogic.CreateFieldImportMaps();
        }

        protected void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            View.Close();
        }

        protected void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var paramObj = (Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase)View.CurrentObject;
            if (paramObj.File.Content == null)
                throw new UserFriendlyException("No file was selected to upload.");
            var byteArray = paramObj.File.Content;
            var stream = new MemoryStream(byteArray);
            importLogic = paramObj.CreateImportLogic(Application, stream);
            importLogic.BeforeImport += OnBeforeImport;
            importLogic.AfterImport += OnAfterImport;
            importLogic.Import();
            View.Close();
        }

        // clean up
        protected virtual void OnAfterImport()
        {
            if (importLogic == null) return;
            importLogic.BeforeImport -= OnBeforeImport;
            importLogic.AfterImport -= OnAfterImport;
        }

        protected virtual void OnBeforeImport()
        {
            // implementation here
        }
    }
}
