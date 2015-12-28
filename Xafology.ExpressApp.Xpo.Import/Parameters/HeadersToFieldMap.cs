using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public class HeadersToFieldMap : Xafology.ExpressApp.Xpo.Import.Parameters.FieldMap
    {
        public HeadersToFieldMap(DevExpress.Xpo.Session session)
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

        private Xafology.ExpressApp.Xpo.Import.Parameters.ImportHeadersParam _ImportCsvFileParam;
        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps")]
        public Xafology.ExpressApp.Xpo.Import.Parameters.ImportHeadersParam ImportCsvFileParam
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
