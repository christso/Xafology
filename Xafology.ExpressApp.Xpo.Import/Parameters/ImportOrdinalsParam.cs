using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.IO;
using Xafology.ExpressApp.Xpo.Import.Logic;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    public class ImportOrdinalsParam : ImportParamBase, ImportParam
    {
        private bool _HasHeaders;

        public ImportOrdinalsParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps"), Aggregated]
        public XPCollection<OrdinalToFieldMap> FieldOrdImportMaps
        {
            get
            {
                return GetCollection<OrdinalToFieldMap>("FieldOrdImportMaps");
            }
        }

        public bool HasHeaders
        {
            get
            {
                return _HasHeaders;
            }
            set
            {
                SetPropertyValue("HasHeaders", ref _HasHeaders, value);
            }
        }

        public override XPBaseCollection FieldMaps
        {
            get { return FieldOrdImportMaps; }
        }

        public override CsvToXpoLoader CreateImportLogic(XafApplication application, Stream stream)
        {
            return new OrdCsvToXpoLoader(application,
                        this, stream);
        }
    }
}
