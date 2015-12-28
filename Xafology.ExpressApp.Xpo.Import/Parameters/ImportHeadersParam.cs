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
    public class ImportHeadersParam : ImportParamBase
    {
        public ImportHeadersParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps"), Aggregated]
        public XPCollection<HeadersToFieldMap> FieldHeadImportMaps
        {
            get
            {
                return GetCollection<HeadersToFieldMap>("FieldHeadImportMaps");
            }
        }

        public override XPBaseCollection FieldImportMaps
        {
            get { return FieldHeadImportMaps; }
        }

        public override CsvToXpoLoader CreateImportLogic(XafApplication application,
            Stream stream)
        {
            return new HeadCsvToXpoLoader(application,
                        this, stream);
        }
    }
}