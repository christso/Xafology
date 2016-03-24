using DevExpress.ExpressApp;
using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class HeaderToFieldMap : FieldMap
    {
        public HeaderToFieldMap(DevExpress.Xpo.Session session)
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

        private ImportHeadersParam _ImportCsvFileParam;
        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps")]
        public ImportHeadersParam ImportCsvFileParam
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
