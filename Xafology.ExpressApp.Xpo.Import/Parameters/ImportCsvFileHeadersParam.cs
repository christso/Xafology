using Xafology.ExpressApp.Concurrency;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;
using Xafology.ExpressApp.Xpo.Import.Logic;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    public class ImportCsvFileHeadersParam : ImportCsvFileParamBase
    {
        public ImportCsvFileHeadersParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps"), Aggregated]
        public XPCollection<CsvFieldHeadersImportMap> FieldHeadImportMaps
        {
            get
            {
                return GetCollection<CsvFieldHeadersImportMap>("FieldHeadImportMaps");
            }
        }

        public override XPBaseCollection FieldImportMaps
        {
            get { return FieldHeadImportMaps; }
        }

        public override ImportCsvFileLogic CreateImportLogic(XafApplication application,
            Stream stream)
        {
            return new ImportCsvFileHeadersLogic(application,
                        this, stream);
        }
    }
}