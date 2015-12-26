using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.IO;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    public class ImportCsvFileOrdinalsParam : Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase
    {
        private bool _HasHeaders;

        public ImportCsvFileOrdinalsParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps"), Aggregated]
        public XPCollection<Xafology.ExpressApp.Xpo.Import.Parameters.CsvFieldOrdinalsImportMap> FieldOrdImportMaps
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

        public override Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileLogic CreateImportLogic(XafApplication application, Stream stream)
        {
            return new Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileOrdinalsLogic(application,
                        this, stream);
        }
    }
}
