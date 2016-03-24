using DevExpress.ExpressApp;
using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class OrdinalToFieldMap : FieldMap
    {
        public OrdinalToFieldMap(Session session)
            : base(session)
        {

        }

        private int _SourceOrdinal;
        public int SourceOrdinal
        {
            get
            {
                return _SourceOrdinal;
            }
            set
            {
                SetPropertyValue("SourceOrdinal", ref _SourceOrdinal, value);
            }
        }

        private ImportOrdinalsParam _ImportCsvFileParam;
        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps")]
        public ImportOrdinalsParam ImportCsvFileParam
        {
            get
            {
                return _ImportCsvFileParam;
            }
            set
            {
                SetPropertyValue("ImportCsvFileParam", ref _ImportCsvFileParam, value);
            }
        }

    }
}
