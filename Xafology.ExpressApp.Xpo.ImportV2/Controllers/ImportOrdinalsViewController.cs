using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
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
    public class ImportOrdinalsViewController : ViewController
    {
        public ImportOrdinalsViewController()
        {
            TargetObjectType = typeof(ImportOrdinalsParam);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            var baseController = Frame.GetController<ImportViewController>();
            baseController.DoImport += DoImport;
            baseController.DoRemap += DoRemap;
            baseController.DoTemplate += DoTemplate;
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
        public void DoImport()
        {
            var param = (ImportOrdinalsParam)View.CurrentObject;

            var csvStream = new MemoryStream();
            param.File.SaveToStream(csvStream);
            csvStream.Position = 0;

            var xpoMapper = new XpoFieldMapper(Application);
            ICsvToXpoLoader loader = null;

            if (param.ImportActionType == ImportActionType.Insert)
                loader = new OrdCsvToXpoInserter(param, csvStream, xpoMapper, null);
            else if (param.ImportActionType == ImportActionType.Update)
                loader = new OrdCsvToXpoUpdater(param, csvStream, xpoMapper, null);
            else
                throw new ArgumentException("Invalid Import Action Type", "ImportActionType");

            loader.Execute();
        }

        public void DoRemap()
        {
            var param = (ImportOrdinalsParam)View.CurrentObject;

            var csvStream = new MemoryStream();
            param.File.SaveToStream(csvStream);
            csvStream.Position = 0;

            var mapCreator = new FieldMapListCreator(csvStream);
            var fieldMaps = param.OrdToFieldMaps;

            mapCreator.AppendFieldMaps(((XPObjectSpace)ObjectSpace).Session, fieldMaps);
        }

        public void DoTemplate()
        {
            var param = (ImportOrdinalsParam)View.CurrentObject;
            param.CreateTemplate();
        }

        private void DoTemplate(object sender, EventArgs e)
        {
            DoTemplate();
        }

        private void DoRemap(object sender, EventArgs e)
        {
            DoRemap();
        }

        private void DoImport(object sender, EventArgs e)
        {
            DoImport();
        }
    }
}
