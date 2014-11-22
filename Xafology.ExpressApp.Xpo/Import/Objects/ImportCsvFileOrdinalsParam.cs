using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.IO;

namespace Xafology.ExpressApp.Xpo.Import
{
    [FileAttachment("File")]
    public class ImportCsvFileOrdinalsParam : ImportCsvFileParamBase
    {
        private bool _HasHeaders;

        public ImportCsvFileOrdinalsParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps"), Aggregated]
        public XPCollection<CsvFieldOrdinalsImportMap> FieldOrdImportMaps
        {
            get
            {
                return GetCollection<CsvFieldOrdinalsImportMap>("FieldOrdImportMaps");
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

        public override XPBaseCollection FieldImportMaps
        {
            get
            {
                return FieldOrdImportMaps;
            }
        }

        public override ImportCsvFileLogic CreateImportLogic(XafApplication application, Stream stream)
        {
            return new ImportCsvFileOrdinalsLogic(application,
                        this, stream);
        }
    }
}
