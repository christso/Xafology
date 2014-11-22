using Xafology.ExpressApp.Xpo.Import.Controllers;

namespace Xafology.ImportDemo.Module.Controllers
{
    public class MyImportDetailViewController : ImportCsvFileDetailViewControllerBase
    {
        protected override void OnBeforeImport()
        {
            base.OnBeforeImport();
        }
        protected override void OnAfterImport()
        {
            base.OnAfterImport();
        }
    }
}
