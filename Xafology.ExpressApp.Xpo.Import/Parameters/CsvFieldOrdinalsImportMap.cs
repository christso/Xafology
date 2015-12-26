using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public class CsvFieldOrdinalsImportMap : Xafology.ExpressApp.Xpo.Import.Parameters.CsvFieldImportMap
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

        private Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileOrdinalsParam _ImportCsvFileParam;
        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps")]
        public Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileOrdinalsParam ImportCsvFileParam
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
