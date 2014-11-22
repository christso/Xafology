using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class CsvFieldHeadersImportMap : CsvFieldImportMap
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

        private ImportCsvFileHeadersParam _ImportCsvFileParam;
        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps")]
        public ImportCsvFileHeadersParam ImportCsvFileParam
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
