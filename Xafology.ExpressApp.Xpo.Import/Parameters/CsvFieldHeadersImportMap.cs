using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public class CsvFieldHeadersImportMap : Xafology.ExpressApp.Xpo.Import.Parameters.CsvFieldImportMap
    {
        public CsvFieldHeadersImportMap(DevExpress.Xpo.Session session)
            : base(session)
        {

        }

        private string _SourceName;
        public string SourceName
        {
            get
            {
                return _SourceName;
            }
            set
            {
                SetPropertyValue("SourceName", ref _SourceName, value);
            }
        }

        private Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileHeadersParam _ImportCsvFileParam;
        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps")]
        public Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileHeadersParam ImportCsvFileParam
        {
            get
            {
                return _ImportCsvFileParam;
            }
            set
            {
                SetPropertyValue("ImportCsvFileDataParam", ref _ImportCsvFileParam, value);
            }
        }
    }
}
