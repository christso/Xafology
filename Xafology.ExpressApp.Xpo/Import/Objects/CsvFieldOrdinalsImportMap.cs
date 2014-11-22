using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class CsvFieldOrdinalsImportMap : CsvFieldImportMap
    {
        public CsvFieldOrdinalsImportMap(Session session)
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

        private ImportCsvFileOrdinalsParam _ImportCsvFileParam;
        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps")]
        public ImportCsvFileOrdinalsParam ImportCsvFileParam
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
